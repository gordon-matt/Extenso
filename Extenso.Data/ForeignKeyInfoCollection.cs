using System.Diagnostics;

namespace Extenso.Data;

[DebuggerDisplay("Count: {Count}")]
public sealed class ForeignKeyInfoCollection : List<ForeignKeyInfo>
{
    public bool Contains(string fkColumnName) => this.Any(x => x.ForeignKeyColumn == fkColumnName);
}