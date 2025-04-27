using System.Diagnostics;

namespace Extenso.Data;

[DebuggerDisplay("{ForeignKeyName}")]
public sealed class ForeignKeyInfo
{
    public string ForeignKeyTable { get; set; }

    public string ForeignKeyColumn { get; set; }

    public string PrimaryKeyTable { get; set; }

    public string PrimaryKeyColumn { get; set; }

    public string PrimaryKeyName { get; set; }

    public string ForeignKeyName { get; set; }

    public ForeignKeyInfo()
    {
    }

    public ForeignKeyInfo(string fkTable, string fkColumn, string pkTable, string pkColumn, string primaryKeyName, string foreignKeyName)
    {
        ForeignKeyTable = fkTable;
        ForeignKeyColumn = fkColumn;
        PrimaryKeyTable = pkTable;
        PrimaryKeyColumn = pkColumn;
        PrimaryKeyName = primaryKeyName;
        ForeignKeyName = foreignKeyName;
    }
}