using System;
using System.Data;

namespace Extenso.Data.Reflection
{
    public static class TypeExtensions
    {
        public static DbType ToDbType(this Type type)
        {
            return DataTypeConvertor.GetDbType(type);
        }

        public static SqlDbType ToSqlDbType(this Type type)
        {
            return DataTypeConvertor.GetSqlDbType(type);
        }
    }
}