using System.Diagnostics;

namespace Extenso.Data.QueryBuilder;

[DebuggerDisplay("{Value}")]
public class SqlLiteral
{
    public string Value { get; set; }

    public SqlLiteral(string value)
    {
        Value = value;
    }
}