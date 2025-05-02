using System.Data;

namespace Extenso.Data.Reflection;

public static class TypeExtensions
{
    public static DbType ToDbType(this Type type) => DataTypeConvertor.GetDbType(type);

    public static SqlDbType ToSqlDbType(this Type type) => DataTypeConvertor.GetSqlDbType(type);
}