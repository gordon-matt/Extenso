using System;
using System.Data;
using Extenso.Data.Common;

namespace Extenso.Data
{
    public static class DataTypeConvertor
    {
        public static Type GetSystemType(DbType dbType)
        {
            return DbTypeConverter.ToSystemType(dbType);
        }

        public static Type GetSystemType(SqlDbType sqlDbType)
        {
            return SqlDbTypeConverter.ToSystemType(sqlDbType);
        }

        public static DbType GetDbType(Type type)
        {
            return SystemTypeConverter.ToDbType(type);
        }

        public static DbType GetDbType(SqlDbType sqlDbType)
        {
            return SqlDbTypeConverter.ToDbType(sqlDbType);
        }

        public static SqlDbType GetSqlDbType(Type type)
        {
            return SystemTypeConverter.ToSqlDbType(type);
        }

        public static SqlDbType GetSqlDbType(DbType dbType)
        {
            return DbTypeConverter.ToSqlDbType(dbType);
        }
    }
}