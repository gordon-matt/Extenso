using System.Data;

namespace Extenso.Data;

/// <summary>
/// Provides extension methods for database types.
/// </summary>
public static class DbTypeExtensions
{
    extension(DbType dbType)
    {
        /// <summary>
        /// Converts the value to the SQL database type.
        /// </summary>
        /// <returns>The SQL database type.</returns>
        public SqlDbType ToSqlDbType() => DataTypeConvertor.GetSqlDbType(dbType);

        /// <summary>
        /// Converts the value to the system type.
        /// </summary>
        /// <returns>The system type.</returns>
        public Type ToSystemType()
        {
            var type = DataTypeConvertor.GetSystemType(dbType);
            return type ?? typeof(object);
        }
    }
}