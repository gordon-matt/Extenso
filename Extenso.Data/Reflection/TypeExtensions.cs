using System.Data;

namespace Extenso.Data.Reflection;

/// <summary>
/// Provides extension methods for types.
/// </summary>
public static class TypeExtensions
{
    extension(Type type)
    {
        /// <summary>
        /// Converts the value to the database type.
        /// </summary>
        /// <returns>The database type.</returns>
        public DbType ToDbType() => DataTypeConvertor.GetDbType(type);

        /// <summary>
        /// Converts the value to the SQL database type.
        /// </summary>
        /// <returns>The SQL database type.</returns>
        public SqlDbType ToSqlDbType() => DataTypeConvertor.GetSqlDbType(type);
    }
}