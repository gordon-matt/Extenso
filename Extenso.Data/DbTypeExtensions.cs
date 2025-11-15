using System.Data;

namespace Extenso.Data;

public static class DbTypeExtensions
{
    extension(DbType dbType)
    {
        public SqlDbType ToSqlDbType() => DataTypeConvertor.GetSqlDbType(dbType);

        public Type ToSystemType()
        {
            var type = DataTypeConvertor.GetSystemType(dbType);
            return type ?? typeof(object);
        }
    }
}