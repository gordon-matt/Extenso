using System.Globalization;
using System.Reflection;

namespace Extenso.KendoGridBinder.Containers;

public class FilterObject
{
    public string Field1 { get; set; }
    public string Operator1 { get; set; }
    public string Value1 { get; set; }
    public string IgnoreCase1 { get; set; }

    public string Field2 { get; set; }
    public string Operator2 { get; set; }
    public string Value2 { get; set; }
    public string IgnoreCase2 { get; set; }

    public string Logic { get; set; }

    public bool IsConjugate => Field2 != null;

    public string LogicToken => Logic switch
    {
        "and" => "&&",
        "or" => "||",
        _ => null,
    };

    private string GetPropertyType(Type type, string field)
    {
        foreach (string part in field.Split('.'))
        {
            if (type == null)
            {
                return null;
            }

            var info = type.GetTypeInfo().GetProperty(part);
            if (info == null)
            {
                return null;
            }

            type = info.PropertyType;
        }

        bool bIsGenericOrNullable = type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);

        return bIsGenericOrNullable ? type.GetGenericArguments()[0].Name.ToLower() : type.Name.ToLower();
    }

    public string GetExpression1<TEntity>() => GetExpression<TEntity>(Field1, Operator1, Value1, IgnoreCase1);

    public string GetExpression2<TEntity>() => GetExpression<TEntity>(Field2, Operator2, Value2, IgnoreCase2);

    private string GetExpression<TEntity>(string field, string op, string param, string ignoreCase)
    {
        string dataType = GetPropertyType(typeof(TEntity), field);
        string caseMod = string.Empty;
        string nullCheck = string.Empty;

        if (dataType == "string")
        {
            param = @"""" + param.ToLower() + @"""";
            caseMod = ".ToLower()"; // always ignore case
            nullCheck = $"{field} != null && ";
        }

        if (dataType == "datetime")
        {
            int i = param.IndexOf("GMT", StringComparison.Ordinal);
            if (i > 0)
            {
                param = param[..i];
            }
            var date = DateTime.Parse(param, new CultureInfo("en-US"));

            string str = $"DateTime({date.Year}, {date.Month}, {date.Day})";
            param = str;
        }

        string exStr = op switch
        {
            "eq" => string.Format("({3}{0}{2} == {1})", field, param, caseMod, nullCheck),
            "neq" => string.Format("({3}{0}{2} != {1})", field, param, caseMod, nullCheck),
            "contains" => string.Format("({3}{0}{2}.Contains({1}))", field, param, caseMod, nullCheck),
            "doesnotcontain" => string.Format("({3}!{0}{2}.Contains({1}))", field, param, caseMod, nullCheck),
            "startswith" => string.Format("({3}{0}{2}.StartsWith({1}))", field, param, caseMod, nullCheck),
            "endswith" => string.Format("({3}{0}{2}.EndsWith({1}))", field, param, caseMod, nullCheck),
            "gte" => string.Format("({3}{0}{2} >= {1})", field, param, caseMod, nullCheck),
            "gt" => string.Format("({3}{0}{2} > {1})", field, param, caseMod, nullCheck),
            "lte" => string.Format("({3}{0}{2} <= {1})", field, param, caseMod, nullCheck),
            "lt" => string.Format("({3}{0}{2} < {1})", field, param, caseMod, nullCheck),
            _ => string.Empty,
        };
        return exStr;
    }
}