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
WHERE TABLE_NAME = '{0}'
AND ""table_schema"" = '{1}';";

            const string CMD_IS_PRIMARY_KEY_FORMAT =
@"SELECT ""column_name""
FROM information_schema.""key_column_usage"" kcu
INNER JOIN information_schema.""table_constraints"" tc ON kcu.""constraint_name"" = tc.""constraint_name""
WHERE kcu.""table_name"" = '{0}'
AND tc.""constraint_type"" = 'PRIMARY KEY'
AND kcu.""table_schema"" = '{1}'";

            var list = new ColumnInfoCollection();

            bool alreadyOpen = (connection.State != ConnectionState.Closed);

            try
            {
                var foreignKeyColumns = connection.GetForeignKeyData(tableName, schema);

                if (!alreadyOpen)
                {
                    connection.Open();
                }

                using (var command = new NpgsqlCommand(string.Format(CMD_COLUMN_INFO_FORMAT, tableName, schema), connection))
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
                                // TODO: SqlDbType won't work for PG!! Need to update this ASAP... get System.Type from PG type
                                string type = reader.GetString(2).ToLowerInvariant();
                                columnInfo.DataTypeNative = type;

                                switch (type)
                                {
                                    case "bigint": columnInfo.DataType = DbType.Int64; break;
                                    case "bigserial": columnInfo.DataType = DbType.Int64; break;
                                    case "bit": columnInfo.DataType = DbType.Boolean; break;
                                    case "bit varying": columnInfo.DataType = DbType.Binary; break;
                                    case "boolean": columnInfo.DataType = DbType.Boolean; break;
                                    case "box": columnInfo.DataType = DbType.Object; break;
                                    case "bytea": columnInfo.DataType = DbType.Binary; break;
                                    case "character": columnInfo.DataType = DbType.String; break;
                                    case "character varying": columnInfo.DataType = DbType.String; break;
                                    case "cidr": columnInfo.DataType = DbType.Object; break;
                                    case "circle": columnInfo.DataType = DbType.Object; break;
                                    case "date": columnInfo.DataType = DbType.Date; break;
                                    case "double precision": columnInfo.DataType = DbType.Double; break;
                                    case "inet": columnInfo.DataType = DbType.Object; break;
                                    case "integer": columnInfo.DataType = DbType.Int32; break;
                                    case "interval": columnInfo.DataType = DbType.Time; break;
                                    case "json": columnInfo.DataType = DbType.String; break;
                                    case "line": columnInfo.DataType = DbType.Object; break;
                                    case "lseg": columnInfo.DataType = DbType.Object; break;
                                    case "macaddr": columnInfo.DataType = DbType.Object; break;
                                    case "money": columnInfo.DataType = DbType.Decimal; break;
                                    case "numeric": columnInfo.DataType = DbType.Decimal; break;
                                    case "path": columnInfo.DataType = DbType.Object; break;
                                    case "point": columnInfo.DataType = DbType.Object; break;
                                    case "polygon": columnInfo.DataType = DbType.Object; break;
                                    case "real": columnInfo.DataType = DbType.Single; break;
                                    case "smallint": columnInfo.DataType = DbType.Int16; break;
                                    case "smallserial": columnInfo.DataType = DbType.Int16; break;
                                    case "serial": columnInfo.DataType = DbType.Int32; break;
                                    case "text": columnInfo.DataType = DbType.String; break;
                                    case "time without time zone": columnInfo.DataType = DbType.Time; break;
                                    case "time with time zone": columnInfo.DataType = DbType.DateTimeOffset; break;
                                    case "timestamp without time zone": columnInfo.DataType = DbType.DateTime; break;
                                    case "timestamp with time zone": columnInfo.DataType = DbType.DateTimeOffset; break;
                                    case "tsquery": columnInfo.DataType = DbType.Object; break;
                                    case "tsvector": columnInfo.DataType = DbType.Object; break;
                                    case "txid_snapshot": columnInfo.DataType = DbType.Object; break;
                                    case "uuid": columnInfo.DataType = DbType.Guid; break;
                                    case "xml": columnInfo.DataType = DbType.Xml; break;
                                    default: columnInfo.DataType = DbType.Object; break;
                                }
                            }
                            catch (ArgumentNullException)
                            {
                                columnInfo.DataType = DbType.Object;
                            }
                            catch (ArgumentException)
                            {
                                columnInfo.DataType = DbType.Object;
                            }

                            if (columnInfo.DefaultValue != null &&
                                columnInfo.DefaultValue.Contains("nextval"))
                            {
                                columnInfo.IsAutoIncremented = true;
                                columnInfo.DefaultValue = string.Empty;
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
                command.CommandText = string.Format(CMD_IS_PRIMARY_KEY_FORMAT, tableName, schema);

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
WHERE FK.""table_name"" = '{0}'
AND FK.""table_schema"" = '{1}'
ORDER BY 1,2,3,4";

            var foreignKeyData = new ForeignKeyInfoCollection();

            bool alreadyOpen = (connection.State != ConnectionState.Closed);

            if (!alreadyOpen)
            {
                connection.Open();
            }

            using (var command = new NpgsqlCommand(string.Format(CMD_FOREIGN_KEYS_FORMAT, tableName, schema), connection))
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

        public static int GetRowCount(this NpgsqlConnection connection, string schema, string tableName)
        {
            return (int)connection.ExecuteScalar<long>($@"SELECT COUNT(*) FROM {schema}.""{tableName}""");
        }

        public static IEnumerable<string> GetTableNames(this NpgsqlConnection connection, bool includeViews = false, string schema = "public")
        {
            string query;
            if (includeViews)
            {
                query =
@"SELECT table_name
FROM information_schema.tables
WHERE table_schema = '{0}'
ORDER BY table_name";
            }
            else
            {
                query =
@"SELECT table_name
FROM information_schema.tables
WHERE table_schema = '{0}'
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
                command.CommandText = string.Format(query, schema);

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
$@"SELECT table_name
FROM information_schema.tables
WHERE table_schema = '{schema}'
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