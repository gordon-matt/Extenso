using System.Data;
using System.Data.OleDb;
using Extenso.Data.Common;

namespace Extenso.Data.OleDb;

public static class OleDbConnectionExtensions
{
    public static ColumnInfoCollection GetColumnData(this OleDbConnection connection, string tableName)
    {
        var columnData = new ColumnInfoCollection();

        object[] restrictions = new object[] { null, null, tableName };
        object[] foreignKeyRestrictions = new object[] { null, null, null, null, null, tableName };

        bool alreadyOpen = connection.State != ConnectionState.Closed;

        if (!alreadyOpen)
        {
            connection.Open();
        }

        using var columnsSchema = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, restrictions);
        using var primaryKeySchema = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Primary_Keys, restrictions);
        using var foreignKeySchema = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Foreign_Keys, foreignKeyRestrictions);

        using var command = connection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandText = $"SELECT * FROM [{tableName}]";
        using var reader = command.ExecuteReader(CommandBehavior.KeyInfo);
        using var schema = reader.GetSchemaTable();

        if (!alreadyOpen)
        {
            connection.Close();
        }

        foreach (DataRow row in columnsSchema.Rows)
        {
            var dataType = (OleDbType)row.Field<int>("DATA_TYPE");

            var columnInfo = new ColumnInfo
            {
                ColumnName = row.Field<string>("COLUMN_NAME"),
                DataType = OleDbTypeConverter.ToDbType(dataType),
                DataTypeNative = dataType.ToString(),
                IsNullable = row.Field<bool>("IS_NULLABLE")
            };

            if (row["CHARACTER_MAXIMUM_LENGTH"] != DBNull.Value)
            {
                columnInfo.MaximumLength = row.Field<long>("CHARACTER_MAXIMUM_LENGTH");
            }
            columnInfo.OrdinalPosition = (int)row.Field<long>("ORDINAL_POSITION");

            if (row.Field<bool>("COLUMN_HASDEFAULT"))
            {
                columnInfo.DefaultValue = row.Field<string>("COLUMN_DEFAULT");
            }

            if (row["NUMERIC_PRECISION"] != DBNull.Value)
            {
                columnInfo.Precision = row.Field<int>("NUMERIC_PRECISION");
            }

            if (row["NUMERIC_SCALE"] != DBNull.Value)
            {
                columnInfo.Scale = row.Field<int>("NUMERIC_SCALE");
            }

            if (primaryKeySchema.Rows.OfType<DataRow>().Any(pkRow =>
                pkRow.Field<string>("COLUMN_NAME").Equals(columnInfo.ColumnName, StringComparison.InvariantCultureIgnoreCase)))
            {
                columnInfo.KeyType = KeyType.PrimaryKey;
            }

            if (columnInfo.KeyType == KeyType.None && foreignKeySchema is not null)
            {
                if (foreignKeySchema.Rows.OfType<DataRow>().Any(fkRow =>
                    fkRow.Field<string>("FK_COLUMN_NAME").Equals(columnInfo.ColumnName, StringComparison.InvariantCultureIgnoreCase)))
                {
                    columnInfo.KeyType = KeyType.ForeignKey;
                }
            }

            var otherRow = schema.Rows.OfType<DataRow>().FirstOrDefault(row =>
                row.Field<string>("BaseTableName").Equals(tableName, StringComparison.InvariantCultureIgnoreCase) &&
                row.Field<string>("ColumnName").Equals(columnInfo.ColumnName, StringComparison.InvariantCultureIgnoreCase));

            if (otherRow is not null)
            {
                columnInfo.IsAutoIncremented = otherRow.Field<bool>("IsAutoIncrement");
            }

            columnData.Add(columnInfo);
        }

        return columnData;
    }

    public static ColumnInfoCollection GetColumnData(this OleDbConnection connection, string tableName, IEnumerable<string> columnNames)
    {
        var data = from x in connection.GetColumnData(tableName)
                   where columnNames.Contains(x.ColumnName)
                   select x;

        var collection = new ColumnInfoCollection();
        collection.AddRange(data);
        return collection;
    }

    public static ForeignKeyInfoCollection GetForeignKeyData(this OleDbConnection connection, string tableName)
    {
        object[] foreignKeyRestrictions = new object[] { null, null, null, null, null, tableName };

        bool alreadyOpen = connection.State != ConnectionState.Closed;

        if (!alreadyOpen)
        {
            connection.Open();
        }

        using var foreignKeySchema = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Foreign_Keys, foreignKeyRestrictions);

        if (!alreadyOpen)
        {
            connection.Close();
        }

        var foreignKeyData = new ForeignKeyInfoCollection();
        foreach (DataRow row in foreignKeySchema.Rows)
        {
            var fkInfo = new ForeignKeyInfo
            {
                ForeignKeyColumn = row.Field<string>("FK_COLUMN_NAME"),
                ForeignKeyName = row.Field<string>("FK_NAME"),
                ForeignKeyTable = row.Field<string>("FK_TABLE_NAME"),
                PrimaryKeyColumn = row.Field<string>("PK_COLUMN_NAME"),
                PrimaryKeyName = row.Field<string>("PK_NAME"),
                PrimaryKeyTable = row.Field<string>("PK_TABLE_NAME")
            };
            foreignKeyData.Add(fkInfo);
        }
        return foreignKeyData;
    }

    public static int GetRowCount(this OleDbConnection connection, string tableName)
    {
        using var commandBuilder = new OleDbCommandBuilder();
        return connection.ExecuteScalar($"SELECT COUNT(*) FROM [{tableName}]");
    }

    public static IEnumerable<string> GetTableNames(this OleDbConnection connection, bool includeViews = false)
    {
        var results = new List<string>();

        bool alreadyOpen = connection.State != ConnectionState.Closed;

        if (!alreadyOpen)
        {
            connection.Open();
        }

        using var tableSchema = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
        foreach (DataRow row in tableSchema.Rows)
        {
            results.Add(row.Field<string>("TABLE_NAME"));
        }

        if (includeViews)
        {
            using var viewSchema = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Views, null);
            foreach (DataRow row in viewSchema.Rows)
            {
                results.Add(row.Field<string>("TABLE_NAME"));
            }
        }

        if (!alreadyOpen)
        {
            connection.Close();
        }

        return results;
    }

    public static IEnumerable<string> GetViewNames(this OleDbConnection connection)
    {
        var results = new List<string>();

        bool alreadyOpen = connection.State != ConnectionState.Closed;

        if (!alreadyOpen)
        {
            connection.Open();
        }

        using var viewSchema = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Views, null);
        foreach (DataRow row in viewSchema.Rows)
        {
            results.Add(row.Field<string>("TABLE_NAME"));
        }

        if (!alreadyOpen)
        {
            connection.Close();
        }

        return results;
    }
}