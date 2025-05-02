using System.Data;
using Extenso.Data.Common;
using MySql.Data.MySqlClient;

namespace Extenso.Data.MySql;

public static class MySqlConnectionExtensions
{
    public static ColumnInfoCollection GetColumnData(this MySqlConnection connection, string tableName)
    {
        const string CMD_COLUMN_INFO_FORMAT =
@"SELECT
	`COLUMN_NAME`,
    `COLUMN_DEFAULT`,
    `COLUMN_TYPE`,
    `CHARACTER_MAXIMUM_LENGTH`,
    `IS_NULLABLE`,
    `ORDINAL_POSITION`,
    `NUMERIC_PRECISION`,
    `NUMERIC_SCALE`
FROM `INFORMATION_SCHEMA`.`COLUMNS`
WHERE `TABLE_SCHEMA` = @DatabaseName
AND `TABLE_NAME` = @TableName;";

        const string CMD_IS_PRIMARY_KEY_FORMAT =
@"SELECT `COLUMN_NAME`
FROM `INFORMATION_SCHEMA`.`COLUMNS`
WHERE `TABLE_SCHEMA` = 'HotelOffer'
AND `TABLE_NAME` = 'airport'
AND `COLUMN_KEY` = 'PRI';";

        var list = new ColumnInfoCollection();

        bool alreadyOpen = connection.State != ConnectionState.Closed;

        try
        {
            var foreignKeyColumns = connection.GetForeignKeyData(tableName);

            if (!alreadyOpen)
            {
                connection.Open();
            }

            using var command = new MySqlCommand(CMD_COLUMN_INFO_FORMAT, connection);
            command.CommandType = CommandType.Text;

            command.Parameters.Add(new MySqlParameter
            {
                Direction = ParameterDirection.Input,
                DbType = DbType.String,
                ParameterName = "@DatabaseName",
                Value = connection.Database
            });

            command.Parameters.Add(new MySqlParameter
            {
                Direction = ParameterDirection.Input,
                DbType = DbType.String,
                ParameterName = "@TableName",
                Value = tableName
            });

            using var reader = command.ExecuteReader();
            ColumnInfo columnInfo = null;

            while (reader.Read())
            {
                columnInfo = new ColumnInfo();

                if (!reader.IsDBNull(0)) { columnInfo.ColumnName = reader.GetString(0); }

                columnInfo.DefaultValue = !reader.IsDBNull(1) ? reader.GetString(1) : string.Empty;

                if (foreignKeyColumns.Contains(columnInfo.ColumnName))
                {
                    columnInfo.KeyType = KeyType.ForeignKey;
                }

                if (!reader.IsDBNull(3)) { columnInfo.MaximumLength = reader.GetInt64(3); }

                try
                {
                    // Based on: https://www.devart.com/dotconnect/mysql/docs/datatypemapping.html
                    string nativeType = reader.GetString(2).ToLowerInvariant();
                    if (nativeType.Contains('('))
                    {
                        nativeType = nativeType.LeftOf('(');
                    }

                    columnInfo.DataTypeNative = nativeType;

                    switch (nativeType)
                    {
                        case "bigint unsigned": columnInfo.DataType = DbType.UInt64; break;
                        case "bigint": columnInfo.DataType = DbType.Int64; break;

                        case "bit":
                            {
                                // TODO: This won't work. Because that is for character max length. We need to add 2 properties to
                                //  ColumnInfo object: "NumericPrecision" and "NumericScale" to be used for numerical data types
                                columnInfo.DataType = columnInfo.MaximumLength == 1 ? DbType.Boolean : DbType.Int64;
                            }
                            break;

                        case "bool": columnInfo.DataType = DbType.Boolean; break;
                        case "boolean": columnInfo.DataType = DbType.Boolean; break;

                        case "char":
                            {
                                if (columnInfo.MaximumLength == 36)
                                {
                                    // This might not be a Guid, but most likely (99% sure) it will be
                                    columnInfo.DataType = DbType.Guid;
                                }
                                else
                                {
                                    columnInfo.DataType = DbType.String;
                                }
                            }
                            break;

                        case "binary": columnInfo.DataType = DbType.Binary; break;
                        case "blob": columnInfo.DataType = DbType.Binary; break;
                        case "char byte": columnInfo.DataType = DbType.Binary; break;
                        case "character varying": columnInfo.DataType = DbType.String; break;
                        case "date": columnInfo.DataType = DbType.Date; break;
                        case "datetime": columnInfo.DataType = DbType.DateTime; break;
                        case "datetimeoffset": columnInfo.DataType = DbType.DateTimeOffset; break;
                        case "dec": columnInfo.DataType = DbType.Decimal; break;
                        case "decimal": columnInfo.DataType = DbType.Decimal; break;
                        case "double unsigned": columnInfo.DataType = DbType.Decimal; break; // Deprecated
                        case "double": columnInfo.DataType = DbType.Double; break;
                        case "enum": columnInfo.DataType = DbType.String; break;
                        case "fixed": columnInfo.DataType = DbType.Decimal; break;
                        case "float unsigned": columnInfo.DataType = DbType.Decimal; break; // Deprecated
                        case "float": columnInfo.DataType = DbType.Single; break;
                        case "geometry": columnInfo.DataType = DbType.Object; break;
                        case "int unsigned": columnInfo.DataType = DbType.UInt32; break;
                        case "int": columnInfo.DataType = DbType.Int32; break;
                        case "integer unsigned": columnInfo.DataType = DbType.UInt32; break;
                        case "integer": columnInfo.DataType = DbType.Int32; break;
                        case "json": columnInfo.DataType = DbType.String; break;
                        case "longblob": columnInfo.DataType = DbType.Binary; break;
                        case "longtext": columnInfo.DataType = DbType.String; break;
                        case "mediumblob": columnInfo.DataType = DbType.Binary; break;
                        case "mediumint": columnInfo.DataType = DbType.Int32; break;
                        case "mediumint unsigned": columnInfo.DataType = DbType.Int32; break;
                        case "mediumtext": columnInfo.DataType = DbType.String; break;
                        case "national char": columnInfo.DataType = DbType.String; break;
                        case "national varchar": columnInfo.DataType = DbType.String; break;
                        case "nchar": columnInfo.DataType = DbType.String; break;
                        case "newdate": columnInfo.DataType = DbType.DateTime; break;
                        case "newdecimal": columnInfo.DataType = DbType.Decimal; break;
                        case "numeric": columnInfo.DataType = DbType.Decimal; break;
                        case "nvarchar": columnInfo.DataType = DbType.String; break;
                        case "real": columnInfo.DataType = DbType.Double; break;
                        case "serial": columnInfo.DataType = DbType.Decimal; break;
                        case "set": columnInfo.DataType = DbType.String; break;
                        case "smallint unsigned": columnInfo.DataType = DbType.UInt16; break;
                        case "smallint": columnInfo.DataType = DbType.Int16; break;
                        case "text": columnInfo.DataType = DbType.String; break;
                        case "time": columnInfo.DataType = DbType.Time; break;
                        case "timestamp": columnInfo.DataType = DbType.DateTime; break;
                        case "tinyblob": columnInfo.DataType = DbType.Binary; break;
                        case "tinyint unsigned": columnInfo.DataType = DbType.Byte; break;
                        case "tinyint": columnInfo.DataType = DbType.SByte; break;
                        case "tinytext": columnInfo.DataType = DbType.String; break;
                        case "varbinary": columnInfo.DataType = DbType.Binary; break;
                        case "varchar": columnInfo.DataType = DbType.String; break;
                        case "varstring": columnInfo.DataType = DbType.String; break;
                        case "year": columnInfo.DataType = DbType.Int16; break;
                        default: columnInfo.DataType = DbType.Object; break;
                    }

                    //columnInfo.DataType = DataTypeConvertor.GetSystemType(reader.GetString(2).ToEnum<SqlDbType>(true));
                }
                catch (ArgumentNullException)
                {
                    columnInfo.DataType = DbType.Object;
                }
                catch (ArgumentException)
                {
                    columnInfo.DataType = DbType.Object;
                }

                if (!reader.IsDBNull(4))
                {
                    columnInfo.IsNullable = !reader.GetString(4).ToUpperInvariant().Equals("NO");
                }

                if (!reader.IsDBNull(5))
                { columnInfo.OrdinalPosition = reader.GetInt32(5); }

                if (!reader.IsDBNull(6))
                { columnInfo.Precision = reader.GetInt32(6); }

                if (!reader.IsDBNull(7))
                { columnInfo.Scale = reader.GetInt32(7); }

                list.Add(columnInfo);
            }
        }
        finally
        {
            if (!alreadyOpen && connection.State != ConnectionState.Closed)
            { connection.Close(); }
        }

        #region Primary Keys

        using (var command = connection.CreateCommand())
        {
            command.CommandType = CommandType.Text;
            command.CommandText = CMD_IS_PRIMARY_KEY_FORMAT;

            command.Parameters.Add(new MySqlParameter
            {
                Direction = ParameterDirection.Input,
                DbType = DbType.String,
                ParameterName = "@DatabaseName",
                Value = connection.Database
            });

            command.Parameters.Add(new MySqlParameter
            {
                Direction = ParameterDirection.Input,
                DbType = DbType.String,
                ParameterName = "@TableName",
                Value = tableName
            });

            alreadyOpen = connection.State != ConnectionState.Closed;

            if (!alreadyOpen)
            {
                connection.Open();
            }

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    string pkColumn = reader.GetString(0);
                    var match = list[pkColumn];
                    if (match != null)
                    {
                        match.KeyType = KeyType.PrimaryKey;
                    }
                }
            }

            if (!alreadyOpen)
            {
                connection.Close();
            }
        }

        #endregion Primary Keys

        return list;
    }

    public static ColumnInfoCollection GetColumnData(this MySqlConnection connection, string tableName, IEnumerable<string> columnNames)
    {
        var data = from x in connection.GetColumnData(tableName)
                   where columnNames.Contains(x.ColumnName)
                   select x;

        var collection = new ColumnInfoCollection();
        collection.AddRange(data);
        return collection;
    }

    public static IEnumerable<string> GetDatabaseNames(this MySqlConnection connection)
    {
        const string CMD_SELECT_DATABASE_NAMES = "SHOW DATABASES;";
        var databaseNames = new List<string>();

        bool alreadyOpen = connection.State != ConnectionState.Closed;

        if (!alreadyOpen)
        {
            connection.Open();
        }

        using (var command = connection.CreateCommand())
        {
            command.CommandType = CommandType.Text;
            command.CommandText = CMD_SELECT_DATABASE_NAMES;

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                databaseNames.Add(reader.GetString(0));
            }
        }

        if (!alreadyOpen)
        {
            connection.Close();
        }

        return databaseNames;
    }

    public static ForeignKeyInfoCollection GetForeignKeyData(this MySqlConnection connection, string tableName)
    {
        const string CMD_FOREIGN_KEYS_FORMAT =
@"SELECT
	TABLE_NAME AS FK_Table,
    COLUMN_NAME AS FK_Column,
    REFERENCED_TABLE_NAME AS PK_Table,
    REFERENCED_COLUMN_NAME AS PK_Column,
    CONSTRAINT_NAME AS Constraint_Name
FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
WHERE TABLE_NAME = @TableName
AND CONSTRAINT_NAME <> 'PRIMARY';";

        var foreignKeyData = new ForeignKeyInfoCollection();

        bool alreadyOpen = connection.State != ConnectionState.Closed;

        if (!alreadyOpen)
        {
            connection.Open();
        }

        using (var command = new MySqlCommand(CMD_FOREIGN_KEYS_FORMAT, connection))
        {
            command.CommandType = CommandType.Text;
            command.Parameters.Add(new MySqlParameter
            {
                Direction = ParameterDirection.Input,
                DbType = DbType.String,
                ParameterName = "@TableName",
                Value = tableName
            });

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                foreignKeyData.Add(new ForeignKeyInfo(
                    reader.IsDBNull(0) ? null : reader.GetString(0),
                    reader.IsDBNull(1) ? null : reader.GetString(1),
                    reader.IsDBNull(2) ? null : reader.GetString(2),
                    reader.IsDBNull(3) ? null : reader.GetString(3),
                    string.Empty,
                    reader.IsDBNull(4) ? null : reader.GetString(4)));
            }
        }

        if (!alreadyOpen)
        {
            connection.Close();
        }

        return foreignKeyData;
    }

    public static int GetRowCount(this MySqlConnection connection, string tableName)
    {
        using var commandBuilder = new MySqlCommandBuilder();
        return (int)connection.ExecuteScalar<long>($"SELECT COUNT(*) FROM {commandBuilder.QuoteIdentifier(tableName)}");
    }

    public static IEnumerable<string> GetTableNames(this MySqlConnection connection, bool includeViews = false) => !string.IsNullOrEmpty(connection.Database) ? connection.GetTableNames(connection.Database, includeViews) : ([]);

    public static IEnumerable<string> GetTableNames(this MySqlConnection connection, string databaseName, bool includeViews = false)
    {
        using var commandBuilder = new MySqlCommandBuilder();
        string query = includeViews
            ? $@"USE {commandBuilder.QuoteIdentifier(databaseName)}; SHOW FULL TABLES IN {commandBuilder.QuoteIdentifier(databaseName)};"
            : $@"USE {commandBuilder.QuoteIdentifier(databaseName)}; SHOW FULL TABLES IN {commandBuilder.QuoteIdentifier(databaseName)} WHERE TABLE_TYPE LIKE 'BASE TABLE';";
        var tables = new List<string>();

        bool alreadyOpen = connection.State != ConnectionState.Closed;

        if (!alreadyOpen)
        {
            connection.Open();
        }

        using (var command = connection.CreateCommand())
        {
            command.CommandType = CommandType.Text;
            command.CommandText = query;

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                tables.Add(reader.GetString(0));
            }
        }

        if (!alreadyOpen)
        {
            connection.Close();
        }

        return tables;
    }

    public static IEnumerable<string> GetViewNames(this MySqlConnection connection) => !string.IsNullOrEmpty(connection.Database) ? connection.GetViewNames(connection.Database) : ([]);

    public static IEnumerable<string> GetViewNames(this MySqlConnection connection, string databaseName)
    {
        var tables = new List<string>();

        bool alreadyOpen = connection.State != ConnectionState.Closed;

        if (!alreadyOpen)
        {
            connection.Open();
        }

        using (var command = connection.CreateCommand())
        {
            command.CommandType = CommandType.Text;
            using var commandBuilder = new MySqlCommandBuilder();
            command.CommandText = $@"USE {commandBuilder.QuoteIdentifier(databaseName)}; SHOW FULL TABLES IN {commandBuilder.QuoteIdentifier(databaseName)} WHERE TABLE_TYPE LIKE 'VIEW';";

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                tables.Add(reader.GetString(0));
            }
        }

        if (!alreadyOpen)
        {
            connection.Close();
        }

        return tables;
    }
}