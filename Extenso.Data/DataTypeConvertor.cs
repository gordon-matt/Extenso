using System.Data;
using Extenso.Data.Common;

namespace Extenso.Data;

/// <summary>
/// Provides data type convertor.
/// </summary>
public static class DataTypeConvertor
{
    /// <summary>
    /// Gets the system type.
    /// </summary>
    /// <param name="dbType">The database type.</param>
    /// <returns>The system type.</returns>
    public static Type GetSystemType(DbType dbType) => DbTypeConverter.ToSystemType(dbType);

    /// <summary>
    /// Gets the system type.
    /// </summary>
    /// <param name="sqlDbType">The SQL database type.</param>
    /// <returns>The system type.</returns>
    public static Type GetSystemType(SqlDbType sqlDbType) => SqlDbTypeConverter.ToSystemType(sqlDbType);

    /// <summary>
    /// Gets the database type.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>The database type.</returns>
    public static DbType GetDbType(Type type) => SystemTypeConverter.ToDbType(type);

    /// <summary>
    /// Gets the database type.
    /// </summary>
    /// <param name="sqlDbType">The SQL database type.</param>
    /// <returns>The database type.</returns>
    public static DbType GetDbType(SqlDbType sqlDbType) => SqlDbTypeConverter.ToDbType(sqlDbType);

    /// <summary>
    /// Gets the SQL database type.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>The SQL database type.</returns>
    public static SqlDbType GetSqlDbType(Type type) => SystemTypeConverter.ToSqlDbType(type);

    /// <summary>
    /// Gets the SQL database type.
    /// </summary>
    /// <param name="dbType">The database type.</param>
    /// <returns>The SQL database type.</returns>
    public static SqlDbType GetSqlDbType(DbType dbType) => DbTypeConverter.ToSqlDbType(dbType);
}