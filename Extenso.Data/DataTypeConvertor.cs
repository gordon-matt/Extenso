using System.Data;
using Extenso.Data.Common;

namespace Extenso.Data;

public static class DataTypeConvertor
{
    public static Type GetSystemType(DbType dbType) => DbTypeConverter.ToSystemType(dbType);

    public static Type GetSystemType(SqlDbType sqlDbType) => SqlDbTypeConverter.ToSystemType(sqlDbType);

    public static DbType GetDbType(Type type) => SystemTypeConverter.ToDbType(type);

    public static DbType GetDbType(SqlDbType sqlDbType) => SqlDbTypeConverter.ToDbType(sqlDbType);

    public static SqlDbType GetSqlDbType(Type type) => SystemTypeConverter.ToSqlDbType(type);

    public static SqlDbType GetSqlDbType(DbType dbType) => DbTypeConverter.ToSqlDbType(dbType);
}