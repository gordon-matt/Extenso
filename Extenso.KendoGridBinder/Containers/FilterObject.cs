using System.Globalization;
using System.Reflection;

namespace Extenso.KendoGridBinder.Containers;

/// <summary>
/// Represents the filter object.
/// </summary>
public class FilterObject
{
    /// <summary>
    /// Gets or sets the field1.
    /// </summary>
    public string Field1 { get; set; }
    /// <summary>
    /// Gets or sets the operator1.
    /// </summary>
    public string Operator1 { get; set; }
    /// <summary>
    /// Gets or sets the value1.
    /// </summary>
    public string Value1 { get; set; }
    /// <summary>
    /// Gets or sets the ignore case1.
    /// </summary>
    public string IgnoreCase1 { get; set; }

    /// <summary>
    /// Gets or sets the field2.
    /// </summary>
    public string Field2 { get; set; }
    /// <summary>
    /// Gets or sets the operator2.
    /// </summary>
    public string Operator2 { get; set; }
    /// <summary>
    /// Gets or sets the value2.
    /// </summary>
    public string Value2 { get; set; }
    /// <summary>
    /// Gets or sets the ignore case2.
    /// </summary>
    public string IgnoreCase2 { get; set; }

    /// <summary>
    /// Gets or sets the logic.
    /// </summary>
    public string Logic { get; set; }

    /// <summary>
    /// Gets a value indicating whether this instance is conjugate.
    /// </summary>
    public bool IsConjugate => Field2 != null;

    /// <summary>
    /// Gets the logic token.
    /// </summary>
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

    /// <summary>
    /// Gets the expression1.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <returns>The expression1.</returns>
    public string GetExpression1<TEntity>() => GetExpression<TEntity>(Field1, Operator1, Value1, IgnoreCase1);

    /// <summary>
    /// Gets the expression2.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <returns>The expression2.</returns>
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