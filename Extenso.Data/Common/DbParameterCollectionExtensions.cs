using System.Data.Common;

namespace Extenso.Data.Common;

/// <summary>
/// Provides extension methods for database parameter collections.
/// </summary>
public static class DbParameterCollectionExtensions
{
    extension(DbParameterCollection parameters)
    {
        /// <summary>
        /// Ensures the database nulls.
        /// </summary>
        public void EnsureDbNulls()
        {
            foreach (DbParameter p in parameters)
            {
                p.Value ??= DBNull.Value;
            }
        }
    }
    extension(IEnumerable<DbParameter> parameters)
    {
        /// <summary>
        /// Ensures the database nulls.
        /// </summary>
        public void EnsureDbNulls()
        {
            foreach (var p in parameters)
            {
                p.Value ??= DBNull.Value;
            }
        }
    }
}