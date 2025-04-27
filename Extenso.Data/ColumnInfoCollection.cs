using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Extenso.Data;

[DebuggerDisplay("Count: {Count}")]
public sealed class ColumnInfoCollection : List<ColumnInfo>
{
    public ColumnInfo this[string name] => this.SingleOrDefault(x => x.ColumnName == name);
}