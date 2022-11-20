using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Extenso.Data.Common
{
    public static class DbParameterCollectionExtensions
    {
        public static void EnsureDbNulls(this IEnumerable<DbParameter> parameters)
        {
            foreach (DbParameter p in parameters)
            {
                p.Value ??= DBNull.Value;
            }
        }

        public static void EnsureDbNulls(this DbParameterCollection parameters)
        {
            foreach (DbParameter p in parameters)
            {
                p.Value ??= DBNull.Value;
            }
        }
    }
}