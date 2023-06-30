using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Extenso.Data.Common;
using Npgsql;

namespace Extenso.Data.Npgsql
{
    public static class NpgsqlConnectionExtensions
    {
        public static ColumnInfoCollection GetColumnData(this NpgsqlConnection connection, string tableName, string schema = "public")
        {
            const string CMD_COLUMN_INFO_FORMAT =
@"SELECT ""column_name"", ""column_default"", ""data_type"", ""character_maximum_length"", ""is_nullable"", ""ordinal_position"", ""numeric_precision"", ""numeric_scale""
FROM information_schema.""columns""
WHERE TABLE_NAME = @TableName
AND ""table_schema"" = @SchemaName;";

            const string CMD_IS_PRIMARY_KEY_FORMAT =
@"SELECT ""column_name""
FROM information_schema.""key_column_usage"" kcu
INNER JOIN information_schema.""table_constraints"" tc ON kcu.""constraint_name"" = tc.""constraint_name""
WHERE kcu.""table_name"" = @TableName
AND tc.""constraint_type"" = 'PRIMARY KEY'
AND kcu.""table_schema"" = @SchemaName";

            var list = new ColumnInfoCollection();

            bool alreadyOpen = (connection.State != ConnectionState.Closed);

            try
            {
                var foreignKeyColumns = connection.GetForeignKeyData(tableName, schema);

                if (!alreadyOpen)
                {
                    connection.Open();
                }

                using (var command = new NpgsqlCommand(CMD_COLUMN_INFO_FORMAT, connection))
                {
                    command.CommandType = CommandType.Text;

                    command.Parameters.Add(new NpgsqlParameter
                    {
                        Direction = ParameterDirection.Input,
                        DbType = DbType.String,
                        ParameterName = "@TableName",
                        Value = tableName
                    });

                    command.Parameters.Add(new NpgsqlParameter
                    {
                        Direction = ParameterDirection.Input,
                        DbType = DbType.String,
                        ParameterName = "@SchemaName",
                        Value = schema
                    });

                    using (var reader = command.ExecuteReader())
                    {
                        ColumnInfo columnInfo = null;

                        while (reader.Read())
                        {
                            columnInfo = new ColumnInfo();

                            if (!reader.IsDBNull(0)) { columnInfo.ColumnName = reader.GetString(0); }

                            if (!reader.IsDBNull(1)) { columnInfo.DefaultValue = reader.GetString(1); }
                            else { columnInfo.DefaultValue = string.Empty; }

                            if (!reader.IsDBNull(3)) { columnInfo.MaximumLength = reader.GetInt64(3); }

                            if (foreignKeyColumns.Contains(columnInfo.ColumnName))
                            {
                                columnInfo.KeyType = KeyType.ForeignKey;
                            }

                            try
                            {
                                string nativeType = reader.GetString(2).ToLowerInvariant();
                                columnInfo.DataTypeNative = nativeType;

                                // https://www.npgsql.org/doc/types/basic.html
                                if (nativeType == "bit")
                                {
                                    if (columnInfo.MaximumLength == 1)
                                    {
                                        columnInfo.DataType = DbType.Boolean;
                                    }
                                    else
                                    {
                                        columnInfo.DataType = DbType.Binary;
                                    }
                                }
                                else
                                {
                                    columnInfo.DataType = nativeType switch
                                    {
                                        "bigint" => DbType.Int64,
                                        "bigserial" => DbType.Int64,
                                        "bit varying" => DbType.Binary,
                                        "boolean" => DbType.Boolean,
                                        "box" => DbType.Object,
                                        "bytea" => DbType.Binary,
                                        "char" => DbType.String,
                                        "character" => DbType.String,
                                        "character varying" => DbType.String,
                                        "cid" => DbType.UInt32,
                                        "cidr" => DbType.Object,
                                        "circle" => DbType.Object,
                                        "citext" => DbType.String,
                                        "date" => DbType.Date,
                                        "double precision" => DbType.Double,
                                        "geometry" => DbType.Object,
                                        "hstore" => DbType.Object,
                                        "inet" => DbType.Object,
                                        "integer" => DbType.Int32,
                                        "interval" => DbType.Time,
                                        "json" => DbType.String,
                                        "jsonb" => DbType.String,
                                        "line" => DbType.Object,
                                        "lseg" => DbType.Object,
                                        "macaddr" => DbType.Object,
                                        "money" => DbType.Decimal,
                                        "name" => DbType.String,
                                        "numeric" => DbType.Decimal,
                                        "oid" => DbType.UInt32,
                                        "oidvector" => DbType.Binary,
                                        "path" => DbType.Object,
                                        "point" => DbType.Object,
                                        "polygon" => DbType.Object,
                                        "real" => DbType.Single,
                                        "record" => DbType.Binary,
                                        "smallint" => DbType.Int16,
                                        "smallserial" => DbType.Int16,
                                        "serial" => DbType.Int32,
                                        "text" => DbType.String,
                                        "time without time zone" => DbType.Time,
                                        "time with time zone" => DbType.DateTimeOffset,
                                        "timestamp without time zone" => DbType.DateTime,
                                        "timestamp with time zone" => DbType.DateTimeOffset,
                                        "tsquery" => DbType.Object,
                                        "tsvector" => DbType.Object,
                                        "txid_snapshot" => DbType.Object,
                                        "uuid" => DbType.Guid,
                                        "xid" => DbType.UInt32,
                                        "xml" => DbType.Xml,
                                        _ => DbType.Object,
                                    };
                                }
                            }
                            catch
                            {
                                columnInfo.DataType = DbType.Object;
                            }

                            if (columnInfo.DefaultValue != null &&
                                columnInfo.DefaultValue.Contains("nextval"))
                            {
                                columnInfo.IsAutoIncremented = true;
                                columnInfo.DefaultValue = string.Empty;
                            }

                            if (!reader.IsDBNull(4))
                            {
                                if (reader.GetString(4).ToUpperInvariant().Equals("NO")) { columnInfo.IsNullable = false; }
                                else { columnInfo.IsNullable = true; }
                            }

                            if (!reader.IsDBNull(5)) { columnInfo.OrdinalPosition = reader.GetInt32(5); }
                            if (!reader.IsDBNull(6)) { columnInfo.Precision = reader.GetInt32(6); }
                            if (!reader.IsDBNull(7)) { columnInfo.Scale = reader.GetInt32(7); }

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
                command.CommandType = CommandType.Text;
                command.CommandText = CMD_IS_PRIMARY_KEY_FORMAT;

                command.Parameters.Add(new NpgsqlParameter
                {
                    Direction = ParameterDirection.Input,
                    DbType = DbType.String,
                    ParameterName = "@TableName",
                    Value = tableName
                });

                command.Parameters.Add(new NpgsqlParameter
                {
                    Direction = ParameterDirection.Input,
                    DbType = DbType.String,
                    ParameterName = "@SchemaName",
                    Value = schema
                });

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

        public static ColumnInfoCollection GetColumnData(this NpgsqlConnection connection, string tableName, IEnumerable<string> columnNames, string schema = "public")
        {
            var filteredColumns = connection
                .GetColumnData(tableName, schema: schema)
                .Where(x => columnNames.Contains(x.ColumnName));

            var collection = new ColumnInfoCollection();
            collection.AddRange(filteredColumns);
            return collection;
        }

        public static IEnumerable<string> GetSchemaNames(this NpgsqlConnection connection)
        {
            const string CMD_SELECT_SCHEMA_NAMES = "SELECT nspname FROM pg_catalog.pg_namespace ORDER BY nspname;";
            var schemaNames = new List<string>();

            bool alreadyOpen = (connection.State != ConnectionState.Closed);

            if (!alreadyOpen)
            {
                connection.Open();
            }

            using (var command = connection.CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandText = CMD_SELECT_SCHEMA_NAMES;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        schemaNames.Add(reader.GetString(0));
                    }
                }
            }

            if (!alreadyOpen)
            {
                connection.Close();
            }

            return schemaNames;
        }

        public static IEnumerable<string> GetDatabaseNames(this NpgsqlConnection connection)
        {
            const string CMD_SELECT_DATABASE_NAMES = "SELECT datname FROM pg_database ORDER BY datname;";
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

        public static ForeignKeyInfoCollection GetForeignKeyData(this NpgsqlConnection connection, string tableName, string schema = "public")
        {
            const string CMD_FOREIGN_KEYS_FORMAT =
@"SELECT
	FK.""table_name"" AS FK_Table,
    CU.""column_name"" AS FK_Column,
    PK.""table_name"" AS PK_Table,
    PT.""column_name"" AS PK_Column,
    C.""constraint_name"" AS Constraint_Name
FROM information_schema.""referential_constraints"" C
INNER JOIN information_schema.""table_constraints"" FK ON C.""constraint_name"" = FK.""constraint_name""
INNER JOIN information_schema.""table_constraints"" PK ON C.""unique_constraint_name"" = PK.""constraint_name""
INNER JOIN information_schema.""key_column_usage"" CU ON C.""constraint_name"" = CU.""constraint_name""
INNER JOIN
(
    SELECT i1.""table_name"", i2.""column_name""
    FROM information_schema.""table_constraints"" i1
    INNER JOIN information_schema.""key_column_usage"" i2 ON i1.""constraint_name"" = i2.""constraint_name""
    WHERE i1.""constraint_type"" = 'PRIMARY KEY'
) PT ON PT.""table_name"" = PK.""table_name""
WHERE FK.""table_name"" = @TableName
AND FK.""table_schema"" = @SchemaName
ORDER BY 1,2,3,4";

            var foreignKeyData = new ForeignKeyInfoCollection();

            bool alreadyOpen = (connection.State != ConnectionState.Closed);

            if (!alreadyOpen)
            {
                connection.Open();
            }

            using (var command = new NpgsqlCommand(CMD_FOREIGN_KEYS_FORMAT, connection))
            {
                command.CommandType = CommandType.Text;

                command.Parameters.Add(new NpgsqlParameter
                {
                    Direction = ParameterDirection.Input,
                    DbType = DbType.String,
                    ParameterName = "@TableName",
                    Value = tableName
                });

                command.Parameters.Add(new NpgsqlParameter
                {
                    Direction = ParameterDirection.Input,
                    DbType = DbType.String,
                    ParameterName = "@SchemaName",
                    Value = schema
                });

                using (var reader = command.ExecuteReader())
                {
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
            }

            if (!alreadyOpen)
            {
                connection.Close();
            }

            return foreignKeyData;
        }

        public static int GetRowCount(this NpgsqlConnection connection, string schema, string tableName)
        {
            var commandBuilder = new NpgsqlCommandBuilder();
            return (int)connection.ExecuteScalar<long>($"SELECT COUNT(*) FROM {commandBuilder.QuoteIdentifier(schema)}.{commandBuilder.QuoteIdentifier(tableName)}");
        }

        public static IEnumerable<string> GetTableNames(this NpgsqlConnection connection, bool includeViews = false, string schema = "public")
        {
            string query;
            if (includeViews)
            {
                query =
@"SELECT table_name
FROM information_schema.tables
WHERE table_schema = @SchemaName
ORDER BY table_name";
            }
            else
            {
                query =
@"SELECT table_name
FROM information_schema.tables
WHERE table_schema = @SchemaName
AND table_type = 'BASE TABLE'
ORDER BY table_name";
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

                command.Parameters.Add(new NpgsqlParameter
                {
                    Direction = ParameterDirection.Input,
                    DbType = DbType.String,
                    ParameterName = "@SchemaName",
                    Value = schema
                });

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

        public static IEnumerable<string> GetViewNames(this NpgsqlConnection connection, string schema = "public")
        {
            string query =
@"SELECT table_name
FROM information_schema.tables
WHERE table_schema = @SchemaName
AND table_type = 'VIEW'
ORDER BY table_name";

            var views = new List<string>();

            bool alreadyOpen = (connection.State != ConnectionState.Closed);

            if (!alreadyOpen)
            {
                connection.Open();
            }

            using (var command = connection.CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandText = query;

                command.Parameters.Add(new NpgsqlParameter
                {
                    Direction = ParameterDirection.Input,
                    DbType = DbType.String,
                    ParameterName = "@SchemaName",
                    Value = schema
                });

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        views.Add(reader.GetString(0));
                    }
                }
            }

            if (!alreadyOpen)
            {
                connection.Close();
            }

            return views;
        }
    }
}