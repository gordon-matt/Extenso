using System;
using System.Data;

namespace Extenso.Data;

public static class DbTypeExtensions
{
    public static SqlDbType ToSqlDbType(this DbType dbType) => DataTypeConvertor.GetSqlDbType(dbType);

    public static Type ToSystemType(this DbType dbType)
    {
        var type = DataTypeConvertor.GetSystemType(dbType);
        return type ?? typeof(object);
    }
}