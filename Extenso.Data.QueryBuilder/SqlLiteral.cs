namespace Extenso.Data.QueryBuilder
{
    public class SqlLiteral
    {
        public string Value { get; set; }

        public SqlLiteral(string value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}