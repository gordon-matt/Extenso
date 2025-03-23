using System.Collections.Generic;
using System.Linq;

namespace Extenso.Data;

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

    public override string ToString() => string.Concat("Count: ", this.Count);
}