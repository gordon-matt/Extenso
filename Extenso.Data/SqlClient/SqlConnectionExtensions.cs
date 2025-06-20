﻿using System.Data;
using System.Reflection;
using Extenso.Collections;
using Extenso.Data.Common;
using Microsoft.Data.SqlClient;

namespace Extenso.Data.SqlClient;

// TODO: Use parameterized queries to prevent the risk of SQL injection attacks. Do the same everywhere else as well..
public static class SqlConnectionExtensions
{
    /// <summary>
    /// Asynchronously copies all rows in the supplied collection to a table on the server specified by destinationTableName.
    /// </summary>
    /// <typeparam name="T">The type of the elements of batch.</typeparam>
    /// <param name="connection">An open SqlConnection to use.</param>
    /// <param name="batch">The data you wish to bulk insert.</param>
    /// <param name="destinationTableName">Name of the destination table on the server.</param>
    /// <param name="mappings">
    /// A collection of tuples used to map object properties to column names. (Key = Property Name, Value = Column Name).
    /// If not supplied, then it is assumed that all column names are the same as the object property names.
    /// </param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static async Task BulkInsertAsync<T>(
        this SqlConnection connection,
        IEnumerable<T> batch,
        string destinationTableName,
        IEnumerable<(string SourceProperty, string DestinationColumn)> mappings = null)
    {
        using var bulkCopy = new SqlBulkCopy(connection);
        bulkCopy.DestinationTableName = destinationTableName;

        if (!mappings.IsNullOrEmpty())
        {
            foreach (var (SourceProperty, DestinationColumn) in mappings)
            {
                bulkCopy.ColumnMappings.Add(SourceProperty, DestinationColumn);
            }
        }
        else
        {
            var propertyNames = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).Select(x => x.Name);
            foreach (string propertyName in propertyNames)
            {
                bulkCopy.ColumnMappings.Add(propertyName, propertyName);
            }
        }

        await bulkCopy.WriteToServerAsync(batch.ToDataTable(convertNullableTypes: true));
    }

    public static ColumnInfoCollection GetColumnData(this SqlConnection connection, string tableName, string schema = "dbo")
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
WHERE TABLE_NAME = @TableName
AND TABLE_SCHEMA = @SchemaName";

        const string CMD_IS_PRIMARY_KEY_FORMAT =
@"SELECT CU.COLUMN_NAME
FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS T, INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE CU
WHERE CU.CONSTRAINT_NAME = T.Constraint_Name
AND CU.TABLE_NAME = T.TABLE_NAME
AND T.CONSTRAINT_TYPE = 'PRIMARY KEY'
AND CU.TABLE_NAME = @TableName
AND T.CONSTRAINT_SCHEMA = @SchemaName";

        var list = new ColumnInfoCollection();

        bool alreadyOpen = connection.State != ConnectionState.Closed;

        try
        {
            var foreignKeyColumns = connection.GetForeignKeyData(tableName, schema);

            if (!alreadyOpen)
            {
                connection.Open();
            }

            using var command = new SqlCommand(CMD_COLUMN_INFO_FORMAT, connection);
            command.CommandType = CommandType.Text;

            command.Parameters.Add(new SqlParameter
            {
                Direction = ParameterDirection.Input,
                DbType = DbType.String,
                ParameterName = "@TableName",
                Value = tableName
            });

            command.Parameters.Add(new SqlParameter
            {
                Direction = ParameterDirection.Input,
                DbType = DbType.String,
                ParameterName = "@SchemaName",
                Value = schema
            });

            using var reader = command.ExecuteReader();
            ColumnInfo columnInfo = null;

            while (reader.Read())
            {
                columnInfo = new ColumnInfo();

                if (!reader.IsDBNull(0))
                { columnInfo.ColumnName = reader.GetString(0); }

                columnInfo.DefaultValue = !reader.IsDBNull(1) ? reader.GetString(1) : string.Empty;

                if (foreignKeyColumns.Contains(columnInfo.ColumnName))
                {
                    columnInfo.KeyType = KeyType.ForeignKey;
                }

                //else
                //{
                try
                {
                    string type = reader.GetString(2);
                    columnInfo.DataTypeNative = type;
                    columnInfo.DataType = DataTypeConvertor.GetDbType(type.ToEnum<SqlDbType>(true));
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
                    columnInfo.IsNullable = !reader.GetString(4).ToUpperInvariant().Equals("NO");
                }

                if (!reader.IsDBNull(5))
                { columnInfo.OrdinalPosition = reader.GetInt32(5); }

                if (!reader.IsDBNull(6))
                { columnInfo.Precision = reader.GetByte(6); }

                if (!reader.IsDBNull(7))
                { columnInfo.Scale = reader.GetInt32(7); }

                if (!reader.IsDBNull(8))
                { columnInfo.IsAutoIncremented = reader.GetInt32(8) == 1; }

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

            command.Parameters.Add(new SqlParameter
            {
                Direction = ParameterDirection.Input,
                DbType = DbType.String,
                ParameterName = "@TableName",
                Value = tableName
            });

            command.Parameters.Add(new SqlParameter
            {
                Direction = ParameterDirection.Input,
                DbType = DbType.String,
                ParameterName = "@SchemaName",
                Value = schema
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

    public static ColumnInfoCollection GetColumnData(this SqlConnection connection, string tableName, IEnumerable<string> columnNames, string schema = "dbo")
    {
        var filteredColumns = connection
            .GetColumnData(tableName, schema)
            .Where(x => columnNames.Contains(x.ColumnName));

        var collection = new ColumnInfoCollection();
        collection.AddRange(filteredColumns);
        return collection;
    }

    public static IEnumerable<string> GetSchemaNames(this SqlConnection connection)
    {
        const string CMD_SELECT_SCHEMA_NAMES =
@"SELECT s.[name] as [Name]
FROM sys.schemas S
WHERE [schema_id] < 1000
ORDER BY S.[name]";

        var schemaNames = new List<string>();

        bool alreadyOpen = connection.State != ConnectionState.Closed;

        if (!alreadyOpen)
        {
            connection.Open();
        }

        using (var command = connection.CreateCommand())
        {
            command.CommandType = CommandType.Text;
            command.CommandText = CMD_SELECT_SCHEMA_NAMES;

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                schemaNames.Add(reader.GetString(0));
            }
        }

        if (!alreadyOpen)
        {
            connection.Close();
        }

        return schemaNames;
    }

    public static IEnumerable<string> GetDatabaseNames(this SqlConnection connection)
    {
        const string CMD_SELECT_DATABASE_NAMES = "SELECT NAME FROM SYS.DATABASES ORDER BY NAME";
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

    public static ForeignKeyInfoCollection GetForeignKeyData(this SqlConnection connection, string tableName, string schema = "dbo")
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
WHERE FK.TABLE_NAME = @TableName
AND C.CONSTRAINT_SCHEMA = @SchemaName
ORDER BY 1,2,3,4";

        var foreignKeyData = new ForeignKeyInfoCollection();
        bool alreadyOpen = connection.State != ConnectionState.Closed;

        if (!alreadyOpen)
        {
            connection.Open();
        }

        using (var command = new SqlCommand(CMD_FOREIGN_KEYS_FORMAT, connection))
        {
            command.CommandType = CommandType.Text;

            command.Parameters.Add(new SqlParameter
            {
                Direction = ParameterDirection.Input,
                DbType = DbType.String,
                ParameterName = "@TableName",
                Value = tableName
            });

            command.Parameters.Add(new SqlParameter
            {
                Direction = ParameterDirection.Input,
                DbType = DbType.String,
                ParameterName = "@SchemaName",
                Value = schema
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

    public static int GetRowCount(this SqlConnection connection, string schema, string tableName)
    {
        using var commandBuilder = new SqlCommandBuilder();
        return connection.ExecuteScalar($"SELECT COUNT(*) FROM {commandBuilder.QuoteIdentifier(schema)}.{commandBuilder.QuoteIdentifier(tableName)}");
    }

    /// <summary>
    /// Get table names for all schemas in the specified database.
    /// </summary>
    /// <param name="connection"></param>
    /// <param name="databaseName"></param>
    /// <param name="includeViews"></param>
    /// <returns></returns>
    public static IEnumerable<string> GetTableNames(this SqlConnection connection, string databaseName = null, bool includeViews = false)
    {
        if (string.IsNullOrEmpty(databaseName))
        {
            databaseName = connection.Database;
        }

        using var commandBuilder = new SqlCommandBuilder();

        string query = includeViews
            ? $@"USE {commandBuilder.QuoteIdentifier(databaseName)};
SELECT CONCAT(SCHEMA_NAME([schema_id]),'.', [name]) AS [name]
FROM sys.Tables
WHERE [name] <> 'sysdiagrams'
UNION
SELECT CONCAT(SCHEMA_NAME([schema_id]),'.', [name]) AS [name]
FROM sys.Views
WHERE [name] <> 'sysdiagrams'
ORDER BY [name]"
            : $@"USE {commandBuilder.QuoteIdentifier(databaseName)};
SELECT CONCAT(SCHEMA_NAME([schema_id]),'.', [name]) AS [name]
FROM sys.Tables
WHERE [name] <> 'sysdiagrams'
ORDER BY [name]";

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

    /// <summary>
    /// Get table names for a specific schema in the database.
    /// </summary>
    /// <param name="connection"></param>
    /// <param name="databaseName"></param>
    /// <param name="schema"></param>
    /// <param name="includeViews"></param>
    /// <returns></returns>
    public static IEnumerable<string> GetTableNamesForSchema(this SqlConnection connection, string schema, string databaseName = null, bool includeViews = false)
    {
        if (string.IsNullOrEmpty(databaseName))
        {
            databaseName = connection.Database;
        }

        using var commandBuilder = new SqlCommandBuilder();

        string query = includeViews
            ? $@"USE {commandBuilder.QuoteIdentifier(databaseName)};
SELECT [name]
FROM sys.Tables
WHERE [name] <> 'sysdiagrams'
AND SCHEMA_NAME([schema_id]) = @SchemaName
UNION
SELECT [name]
FROM sys.Views
WHERE [name] <> 'sysdiagrams'
AND SCHEMA_NAME([schema_id]) = @SchemaName
ORDER BY [name]"
            : $@"USE {commandBuilder.QuoteIdentifier(databaseName)};
SELECT [name]
FROM sys.Tables
WHERE [name] <> 'sysdiagrams'
AND SCHEMA_NAME([schema_id]) = @SchemaName
ORDER BY [name]";

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

            command.Parameters.Add(new SqlParameter
            {
                Direction = ParameterDirection.Input,
                DbType = DbType.String,
                ParameterName = "@SchemaName",
                Value = schema
            });

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

    /// <summary>
    /// Get view names for all schemas in the specified database.
    /// </summary>
    /// <param name="connection"></param>
    /// <param name="databaseName"></param>
    /// <returns></returns>
    public static IEnumerable<string> GetViewNames(this SqlConnection connection, string databaseName = null)
    {
        if (string.IsNullOrEmpty(databaseName))
        {
            databaseName = connection.Database;
        }

        using var commandBuilder = new SqlCommandBuilder();

        string query =
$@"USE {commandBuilder.QuoteIdentifier(databaseName)};
SELECT CONCAT(SCHEMA_NAME([schema_id]),'.', [name]) AS [name]
FROM sys.Views
WHERE [name] <> 'sysdiagrams'
ORDER BY [name]";

        var views = new List<string>();

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
                views.Add(reader.GetString(0));
            }
        }

        if (!alreadyOpen)
        {
            connection.Close();
        }

        return views;
    }

    /// <summary>
    /// Get view names for a specific schema in the database.
    /// </summary>
    /// <param name="connection"></param>
    /// <param name="schema"></param>
    /// <param name="databaseName"></param>
    /// <returns></returns>
    public static IEnumerable<string> GetViewNamesForSchema(this SqlConnection connection, string schema, string databaseName = null)
    {
        if (string.IsNullOrEmpty(databaseName))
        {
            databaseName = connection.Database;
        }

        using var commandBuilder = new SqlCommandBuilder();

        string query =
$@"USE {commandBuilder.QuoteIdentifier(databaseName)};
SELECT [name]
FROM sys.Views
WHERE [name] <> 'sysdiagrams'
AND SCHEMA_NAME([schema_id]) = @SchemaName
ORDER BY [name]";

        var views = new List<string>();

        bool alreadyOpen = connection.State != ConnectionState.Closed;

        if (!alreadyOpen)
        {
            connection.Open();
        }

        using (var command = connection.CreateCommand())
        {
            command.CommandType = CommandType.Text;
            command.CommandText = query;

            command.Parameters.Add(new SqlParameter
            {
                Direction = ParameterDirection.Input,
                DbType = DbType.String,
                ParameterName = "@SchemaName",
                Value = schema
            });

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                views.Add(reader.GetString(0));
            }
        }

        if (!alreadyOpen)
        {
            connection.Close();
        }

        return views;
    }
}