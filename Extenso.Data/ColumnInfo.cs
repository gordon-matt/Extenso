using System.Data;

namespace Extenso.Data
{
    // TODO: Should take into account that sometimes a column can be both a primary and foreign key (composite keys)
    public enum KeyType : byte
    {
        None = 0,
        PrimaryKey = 1,
        ForeignKey = 2
    }

    public class ColumnInfo
    {
        public string ColumnName { get; set; }

        public int OrdinalPosition { get; set; }

        public string DefaultValue { get; set; }

        public bool IsNullable { get; set; }

        public DbType DataType { get; set; }

        public string DataTypeNative { get; set; }

        public long MaximumLength { get; set; }

        public int Precision { get; set; }

        public int Scale { get; set; }

        public KeyType KeyType { get; set; }

        /// <summary>
        /// MSSQL: IDENTITY, PG: Sequence, etc..
        /// </summary>
        public bool IsAutoIncremented { get; set; }

        public override string ToString() => ColumnName;
    }
}