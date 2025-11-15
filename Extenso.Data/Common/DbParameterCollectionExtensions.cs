using System.Data.Common;

namespace Extenso.Data.Common;

public static class DbParameterCollectionExtensions
{
    extension(DbParameterCollection parameters)
    {
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
        public void EnsureDbNulls()
        {
            foreach (var p in parameters)
            {
                p.Value ??= DBNull.Value;
            }
        }
    }
}