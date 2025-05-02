using System.Diagnostics;

namespace Extenso.Data;

[DebuggerDisplay("Count: {Count}")]
public sealed class ForeignKeyInfoCollection : List<ForeignKeyInfo>
{
    public bool Contains(string fkColumnName)
    {
        foreach (string fkColumn in this.Select(x => x.ForeignKeyColumn))
        {
            if (fkColumn == fkColumnName)
            { return true; }
        }
        return false;
    }
}