using System.Data;

namespace Extenso.Data.Reflection;

public static class TypeExtensions
{
    extension(Type type)
    {
        public DbType ToDbType() => DataTypeConvertor.GetDbType(type);

        public SqlDbType ToSqlDbType() => DataTypeConvertor.GetSqlDbType(type);
    }
}