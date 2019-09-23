using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Extenso.Data.Common;

namespace Extenso.Data.SqlClient
{
    public static class SqlConnectionExtensions
    {
        public static IEnumerable<string> GetDatabaseNames(this SqlConnection connection)
        {
            const string CMD_SELECT_DATABASE_NAMES = "SELECT NAME FROM SYS.DATABASES ORDER BY NAME";
            var databaseNames = new List<string>();

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

        public static ForeignKeyInfoCollection GetForeignKeyData(this SqlConnection connection, string tableName)
        {
            const string CMD_FOREIGN_KEYS_FORMAT =
@"SELECT FK_Table = FK.TABLE_NAME,
    FK_Column = CU.COLUMN_NAME,
	PK_Table = PK.TABLE_NAME,
    PK_Column = PT.COLUMN_NAME,
	Constraint_Name = C.CONSTRAINT_NAME
FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS C
INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS FK ON C.CONSTRAINT_NAME = FK.CONSTRAINT_NAME
INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS PK ON C.UNIQUE_CONSTRAINT_NAME = PK.CONSTRAINT_NAME
INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE CU ON C.CONSTRAINT_NAME = CU.CONSTRAINT_NAME
INNER JOIN
(
	SELECT i1.TABLE_NAME, i2.COLUMN_NAME
	FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS i1
	INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE i2 ON
		i1.CONSTRAINT_NAME = i2.CONSTRAINT_NAME
	WHERE i1.CONSTRAINT_TYPE = 'PRIMARY KEY'
) PT ON PT.TABLE_NAME = PK.TABLE_NAME
WHERE FK.TABLE_NAME = '{0}'
ORDER BY 1,2,3,4";

            var foreignKeyData = new ForeignKeyInfoCollection();

            bool alreadyOpen = (connection.State != ConnectionState.Closed);

            if (!alreadyOpen)
            {
                connection.Open();
            }

            using (var command = new SqlCommand(string.Format(CMD_FOREIGN_KEYS_FORMAT, tableName), connection))
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

        public static int GetRowCount(this SqlConnection connection, string tableName)
        {
            return connection.ExecuteScalar($"SELECT COUNT(*) FROM [{tableName}]");
        }

        public static IEnumerable<string> GetTableNames(this SqlConnection connection, bool includeViews = false)
        {
            if (!string.IsNullOrEmpty(connection.Database))
            {
                return connection.GetTableNames(connection.Database, includeViews);
            }
            else { return new List<string>(); }
        }

        public static IEnumerable<string> GetTableNames(this SqlConnection connection, string databaseName, bool includeViews = false)
        {
            string query = string.Empty;

            if (includeViews)
            {
                query = $"USE {databaseName} SELECT [name] FROM sys.Tables WHERE [name] <> 'sysdiagrams' UNION SELECT [name] FROM sys.Views WHERE [name] <> 'sysdiagrams' ORDER BY [name]";
            }
            else
            {
                query = $"USE {databaseName} SELECT [name] FROM sys.Tables WHERE [name] <> 'sysdiagrams' ORDER BY [name]";
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

        public static IEnumerable<string> GetViewNames(this SqlConnection connection)
        {
            if (!string.IsNullOrEmpty(connection.Database))
            {
                return connection.GetViewNames(connection.Database);
            }
            else { return new List<string>(); }
        }

        public static IEnumerable<string> GetViewNames(this SqlConnection connection, string databaseName)
        {
            string query = $"USE {databaseName} [name] FROM sys.Views WHERE [name] <> 'sysdiagrams' ORDER BY [name]";

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

        public static ColumnInfoCollection GetColumnData(this SqlConnection connection, string tableName)
        {
            const string CMD_COLUMN_INFO_FORMAT =
@"SELECT
    COLUMN_NAME,
    COLUMN_DEFAULT,
    DATA_TYPE,
    CHARACTER_MAXIMUM_LENGTH,
    IS_NULLABLE,
    ORDINAL_POSITION,
    NUMERIC_PRECISION,
    NUMERIC_SCALE,
    COLUMNPROPERTY(object_id(TABLE_SCHEMA + '.' + TABLE_NAME), COLUMN_NAME, 'IsIdentity') AS 'IsIdentity'
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = '{0}'";

            const string CMD_IS_PRIMARY_KEY_FORMAT =
@"SELECT CU.COLUMN_NAME
FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS T, INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE CU
WHERE CU.CONSTRAINT_NAME = T.Constraint_Name
AND CU.TABLE_NAME = T.TABLE_NAME
AND CONSTRAINT_TYPE = 'PRIMARY KEY'
AND CU.TABLE_NAME = '{0}'";

            var list = new ColumnInfoCollection();

            bool alreadyOpen = (connection.State != ConnectionState.Closed);

            try
            {
                var foreignKeyColumns = connection.GetForeignKeyData(tableName);

                if (!alreadyOpen)
                {
                    connection.Open();
                }

                using (var command = new SqlCommand(string.Format(CMD_COLUMN_INFO_FORMAT, tableName), connection))
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

                            //else
                            //{
                            try
                            {
                                columnInfo.DataType = DataTypeConvertor.GetDbType(reader.GetString(2).ToEnum<SqlDbType>(true));
                            }
                            catch (ArgumentNullException)
                            {
                                columnInfo.DataType = DbType.Object;
                            }
                            catch (ArgumentException)
                            {
                                columnInfo.DataType = DbType.Object;
                            }

                            //}

                            if (!reader.IsDBNull(3))
                            { columnInfo.MaximumLength = reader.GetInt32(3); }

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
                            { columnInfo.Precision = reader.GetByte(6); }

                            if (!reader.IsDBNull(7))
                            { columnInfo.Scale = reader.GetInt32(7); }

                            if (!reader.IsDBNull(8))
                            { columnInfo.IsAutoIncremented = reader.GetInt32(8) == 1 ? true : false; }

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

        public static ColumnInfoCollection GetColumnData(this SqlConnection connection, string tableName, IEnumerable<string> columnNames)
        {
            var collection = new ColumnInfoCollection();

            var filteredColumns = connection
                .GetColumnData(tableName)
                .Where(x => columnNames.Contains(x.ColumnName));

            collection.AddRange(filteredColumns);
            return collection;
        }
    }
}