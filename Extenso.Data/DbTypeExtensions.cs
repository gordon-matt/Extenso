using System;
using System.Data;

namespace Extenso.Data
{
    public static class DbTypeExtensions
    {
        public static SqlDbType ToSqlDbType(this DbType dbType)
        {
            return DataTypeConvertor.GetSqlDbType(dbType);
        }

        public static Type ToSystemType(this DbType dbType)
        {
            var type = DataTypeConvertor.GetSystemType(dbType);
            return type == null ? typeof(object) : type;
        }
    }
}