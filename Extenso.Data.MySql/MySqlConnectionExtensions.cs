using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Extenso.Data.Common;
using MySql.Data.MySqlClient;

namespace Extenso.Data.MySql
{
    public static class MySqlConnectionExtensions
    {
        public static ColumnInfoCollection GetColumnData(this MySqlConnection connection, string tableName)
        {
            const string CMD_COLUMN_INFO_FORMAT =
@"SELECT column_name, column_default, data_type, character_maximum_length, is_nullable, ordinal_position, numeric_precision, numeric_scale
FROM information_schema.columns
WHERE table_name = '{0}';";

            const string CMD_IS_PRIMARY_KEY_FORMAT =
@"SELECT column_name
FROM information_schema.key_column_usage kcu
INNER JOIN information_schema.table_constraints tc ON kcu.constraint_name = tc.constraint_name
WHERE kcu.table_name = '{0}'
AND tc.constraint_type = 'PRIMARY KEY'";

            ColumnInfoCollection list = new ColumnInfoCollection();

            bool alreadyOpen = (connection.State != ConnectionState.Closed);

            try
            {
                ForeignKeyInfoCollection foreignKeyColumns = connection.GetForeignKeyData(tableName);

                if (!alreadyOpen)
                {
                    connection.Open();
                }

                using (var command = new MySqlCommand(string.Format(CMD_COLUMN_INFO_FORMAT, tableName), connection))
                {
                    command.CommandType = CommandType.Text;
                    using (var reader = command.ExecuteReader())
                    {
                        ColumnInfo columnInfo = null;

                        while (reader.Read())
                        {
                            columnInfo = new ColumnInfo();

                            if (!reader.IsDBNull(0))
                            { columnInfo.ColumnName = reader.GetString(0); }

                            if (!reader.IsDBNull(1))
                            { columnInfo.DefaultValue = reader.GetString(1); }
                            else
                            { columnInfo.DefaultValue = string.Empty; }

                            if (foreignKeyColumns.Contains(columnInfo.ColumnName))
                            {
                                columnInfo.KeyType = KeyType.ForeignKey;
                            }

                            if (!reader.IsDBNull(3))
                            { columnInfo.MaximumLength = reader.GetInt32(3); }

                            try
                            {
                                // Based on: https://www.devart.com/dotconnect/mysql/docs/datatypemapping.html
                                string type = reader.GetString(2).ToLowerInvariant();
                                columnInfo.DataTypeNative = type;

                                switch (type)
                                {
                                    case "bool": columnInfo.DataType = DbType.Boolean; break;
                                    case "boolean": columnInfo.DataType = DbType.Boolean; break;

                                    case "bit":
                                        {
                                            // TODO: This won't work. Because that is for character max length. We need to add 2 properties to
                                            //  ColumnInfo object: "NumericPrecision" and "NumericScale" to be used for numerical data types
                                            if (columnInfo.MaximumLength == 1)
                                            {
                                                columnInfo.DataType = DbType.Boolean;
                                            }
                                            else
                                            {
                                                columnInfo.DataType = DbType.Int64;
                                            }
                                        }
                                        break;

                                    case "tinyint": columnInfo.DataType = DbType.SByte; break;
                                    case "tinyint unsigned": columnInfo.DataType = DbType.Byte; break;
                                    case "smallint": columnInfo.DataType = DbType.Int16; break;
                                    case "year": columnInfo.DataType = DbType.Int16; break;
                                    case "int": columnInfo.DataType = DbType.Int32; break;
                                    case "integer": columnInfo.DataType = DbType.Int32; break;
                                    case "smallint unsigned": columnInfo.DataType = DbType.UInt16; break;
                                    case "mediumint": columnInfo.DataType = DbType.Int32; break;
                                    case "bigint": columnInfo.DataType = DbType.Int64; break;
                                    case "int unsigned": columnInfo.DataType = DbType.UInt32; break;
                                    case "integer unsigned": columnInfo.DataType = DbType.UInt32; break;
                                    case "float": columnInfo.DataType = DbType.Single; break;
                                    case "double": columnInfo.DataType = DbType.Double; break;
                                    case "real": columnInfo.DataType = DbType.Double; break;
                                    case "decimal": columnInfo.DataType = DbType.Decimal; break;
                                    case "numeric": columnInfo.DataType = DbType.Decimal; break;
                                    case "dec": columnInfo.DataType = DbType.Decimal; break;
                                    case "fixed": columnInfo.DataType = DbType.Decimal; break;
                                    case "bigint unsigned": columnInfo.DataType = DbType.UInt64; break;
                                    case "float unsigned": columnInfo.DataType = DbType.Decimal; break;
                                    case "double unsigned": columnInfo.DataType = DbType.Decimal; break;
                                    case "serial": columnInfo.DataType = DbType.Decimal; break;
                                    case "date": columnInfo.DataType = DbType.DateTime; break;
                                    case "timestamp": columnInfo.DataType = DbType.DateTime; break;
                                    case "datetime": columnInfo.DataType = DbType.DateTime; break;
                                    case "datetimeoffset": columnInfo.DataType = DbType.DateTimeOffset; break;
                                    case "time": columnInfo.DataType = DbType.Time; break;

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

                                    case "varchar": columnInfo.DataType = DbType.String; break;
                                    case "tinytext": columnInfo.DataType = DbType.String; break;
                                    case "text": columnInfo.DataType = DbType.String; break;
                                    case "mediumtext": columnInfo.DataType = DbType.String; break;
                                    case "longtext": columnInfo.DataType = DbType.String; break;
                                    case "set": columnInfo.DataType = DbType.String; break;
                                    case "enum": columnInfo.DataType = DbType.String; break;
                                    case "nchar": columnInfo.DataType = DbType.String; break;
                                    case "national char": columnInfo.DataType = DbType.String; break;
                                    case "nvarchar": columnInfo.DataType = DbType.String; break;
                                    case "national varchar": columnInfo.DataType = DbType.String; break;
                                    case "character varying": columnInfo.DataType = DbType.String; break;
                                    case "binary": columnInfo.DataType = DbType.Binary; break;
                                    case "varbinary": columnInfo.DataType = DbType.Binary; break;
                                    case "tinyblob": columnInfo.DataType = DbType.Binary; break;
                                    case "blob": columnInfo.DataType = DbType.Binary; break;
                                    case "mediumblob": columnInfo.DataType = DbType.Binary; break;
                                    case "longblob": columnInfo.DataType = DbType.Binary; break;
                                    case "char byte": columnInfo.DataType = DbType.Binary; break;
                                    case "geometry": columnInfo.DataType = DbType.Object; break;
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
                                if (reader.GetString(4).ToUpperInvariant().Equals("NO"))
                                { columnInfo.IsNullable = false; }
                                else
                                { columnInfo.IsNullable = true; }
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
                command.CommandText = string.Format(CMD_IS_PRIMARY_KEY_FORMAT, tableName);

                alreadyOpen = (connection.State != ConnectionState.Closed);

                if (!alreadyOpen)
                {
                    connection.Open();
                }

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string pkColumn = reader.GetString(0);
                        ColumnInfo match = list[pkColumn];
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
            List<string> databaseNames = new List<string>();

            bool alreadyOpen = (connection.State != ConnectionState.Closed);

            if (!alreadyOpen)
            {
                connection.Open();
            }

            using (var command = connection.CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandText = CMD_SELECT_DATABASE_NAMES;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        databaseNames.Add(reader.GetString(0));
                    }
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
WHERE TABLE_NAME = 'kore_blogposts'
AND CONSTRAINT_NAME <> 'PRIMARY';";

            var foreignKeyData = new ForeignKeyInfoCollection();

            bool alreadyOpen = (connection.State != ConnectionState.Closed);

            if (!alreadyOpen)
            {
                connection.Open();
            }

            using (var command = new MySqlCommand(string.Format(CMD_FOREIGN_KEYS_FORMAT, tableName), connection))
            {
                command.CommandType = CommandType.Text;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        foreignKeyData.Add(new ForeignKeyInfo(
                            reader.GetString(0),
                            reader.GetString(1),
                            reader.GetString(2),
                            reader.GetString(3),
                            string.Empty,
                            reader.GetString(4)));
                    }
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
            return connection.ExecuteScalar(string.Format("SELECT COUNT(*) FROM `{0}`", tableName));
        }

        public static IEnumerable<string> GetTableNames(this MySqlConnection connection, bool includeViews = false)
        {
            if (!string.IsNullOrEmpty(connection.Database))
            {
                return connection.GetTableNames(connection.Database, includeViews);
            }
            else { return new List<string>(); }
        }

        public static IEnumerable<string> GetTableNames(this MySqlConnection connection, string databaseName, bool includeViews = false)
        {
            string query = string.Empty;

            if (includeViews)
            {
                query = string.Concat("USE ", databaseName, "; SHOW FULL TABLES IN ", connection.Database);
            }
            else
            {
                query = string.Concat("USE ", databaseName, "; SHOW FULL TABLES IN ", connection.Database, " WHERE TABLE_TYPE LIKE 'BASE TABLE';");
            }

            var tables = new List<string>();

            bool alreadyOpen = (connection.State != ConnectionState.Closed);

            if (!alreadyOpen)
            {
                connection.Open();
            }

            using (var command = connection.CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandText = query;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tables.Add(reader.GetString(0));
                    }
                }
            }

            if (!alreadyOpen)
            {
                connection.Close();
            }

            return tables;
        }

        public static IEnumerable<string> GetViewNames(this MySqlConnection connection)
        {
            if (!string.IsNullOrEmpty(connection.Database))
            {
                return connection.GetViewNames(connection.Database);
            }
            else { return new List<string>(); }
        }

        public static IEnumerable<string> GetViewNames(this MySqlConnection connection, string databaseName)
        {
            string query = string.Concat("USE ", databaseName, "; SHOW FULL TABLES IN ", connection.Database, " WHERE TABLE_TYPE LIKE 'VIEW';");

            var tables = new List<string>();

            bool alreadyOpen = (connection.State != ConnectionState.Closed);

            if (!alreadyOpen)
            {
                connection.Open();
            }

            using (var command = connection.CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandText = query;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tables.Add(reader.GetString(0));
                    }
                }
            }

            if (!alreadyOpen)
            {
                connection.Close();
            }

            return tables;
        }
    }
}