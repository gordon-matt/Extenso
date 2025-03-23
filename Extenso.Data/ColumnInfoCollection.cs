using System.Collections.Generic;
using System.Linq;

namespace Extenso.Data;

public sealed class ColumnInfoCollection : List<ColumnInfo>
{
    public ColumnInfo this[string name] => this.SingleOrDefault(x => x.ColumnName == name);

    public override string ToString() => string.Concat("Count: ", Count);
}