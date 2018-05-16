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
            List<string> databaseNames = new List<string>();

            bool alreadyOpen = (connection.State != ConnectionState.Closed);

            if (!alreadyOpen)
            {
                connection.Open();
            }

            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandText = CMD_SELECT_DATABASE_NAMES;

                using (SqlDataReader reader = command.ExecuteReader())
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
            #region Testing

            //string oleConnectionString = connection.ConnectionString;

            //if (!oleConnectionString.Contains("Provider"))
            //{
            //    oleConnectionString = oleConnectionString.Prepend("Provider=SQLOLEDB;");
            //}

            //using (OleDbConnection oleConnection = new OleDbConnection(oleConnectionString))
            //{
            //    return oleConnection.GetForeignKeyData(tableName);
            //}

            #endregion Testing

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

            ForeignKeyInfoCollection foreignKeyData = new ForeignKeyInfoCollection();

            bool alreadyOpen = (connection.State != ConnectionState.Closed);

            if (!alreadyOpen)
            {
                connection.Open();
            }

            using (SqlCommand command = new SqlCommand(string.Format(CMD_FOREIGN_KEYS_FORMAT, tableName), connection))
            {
                command.CommandType = CommandType.Text;
                using (SqlDataReader reader = command.ExecuteReader())
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
            return connection.ExecuteScalar(string.Concat("SELECT COUNT(*) FROM ", tableName));
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
                query = string.Concat("USE ", databaseName, " SELECT [name] FROM sys.Tables WHERE [name] <> 'sysdiagrams' UNION SELECT [name] FROM sys.Views WHERE [name] <> 'sysdiagrams' ORDER BY [name]");
            }
            else
            {
                query = string.Concat("USE ", databaseName, " SELECT [name] FROM sys.Tables WHERE [name] <> 'sysdiagrams' ORDER BY [name]");
            }

            List<string> tables = new List<string>();

            bool alreadyOpen = (connection.State != ConnectionState.Closed);

            if (!alreadyOpen)
            {
                connection.Open();
            }

            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandText = query;

                using (SqlDataReader reader = command.ExecuteReader())
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
            string query = string.Concat("USE ", databaseName, " SELECT [name] FROM sys.Views WHERE [name] <> 'sysdiagrams' ORDER BY [name]");

            List<string> tables = new List<string>();

            bool alreadyOpen = (connection.State != ConnectionState.Closed);

            if (!alreadyOpen)
            {
                connection.Open();
            }

            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandText = query;

                using (SqlDataReader reader = command.ExecuteReader())
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
            #region Testing

            //string oleConnectionString = connection.ConnectionString;

            //if (!oleConnectionString.Contains("Provider"))
            //{
            //    oleConnectionString = oleConnectionString.Prepend("Provider=SQLOLEDB;");
            //}

            //using (OleDbConnection oleConnection = new OleDbConnection(oleConnectionString))
            //{
            //    return oleConnection.GetColumnData(tableName);
            //}

            #endregion Testing

            const string CMD_COLUMN_INFO_FORMAT =
@"SELECT COLUMN_NAME, COLUMN_DEFAULT, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH, IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = '{0}'";

            const string CMD_IS_PRIMARY_KEY_FORMAT =
@"SELECT COLUMN_NAME
FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
WHERE OBJECTPROPERTY(OBJECT_ID(CONSTRAINT_NAME), 'IsPrimaryKey') = 1
AND TABLE_NAME = '{0}'";

            ColumnInfoCollection list = new ColumnInfoCollection();

            bool alreadyOpen = (connection.State != ConnectionState.Closed);

            try
            {
                ForeignKeyInfoCollection foreignKeyColumns = connection.GetForeignKeyData(tableName);

                if (!alreadyOpen)
                {
                    connection.Open();
                }

                using (SqlCommand command = new SqlCommand(string.Format(CMD_COLUMN_INFO_FORMAT, tableName), connection))
                {
                    command.CommandType = CommandType.Text;
                    using (SqlDataReader reader = command.ExecuteReader())
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
                                columnInfo.DataType = DataTypeConvertor.GetSystemType(reader.GetString(2).ToEnum<SqlDbType>(true));
                            }
                            catch (ArgumentNullException)
                            {
                                columnInfo.DataType = typeof(object);
                            }
                            catch (ArgumentException)
                            {
                                columnInfo.DataType = typeof(object);
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

            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = string.Format(CMD_IS_PRIMARY_KEY_FORMAT, tableName);

                alreadyOpen = (connection.State != ConnectionState.Closed);

                if (!alreadyOpen)
                {
                    connection.Open();
                }

                using (SqlDataReader reader = command.ExecuteReader())
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
            var data = from x in connection.GetColumnData(tableName)
                       where columnNames.Contains(x.ColumnName)
                       select x;

            ColumnInfoCollection collection = new ColumnInfoCollection();
            collection.AddRange(data);
            return collection;
        }
    }
}