using System;
using System.Data.Common;

namespace Extenso.Data.Common
{
    public static class DbParameterCollectionExtensions
    {
        public static void EnsureDbNulls(this DbParameterCollection parameters)
        {
            foreach (DbParameter p in parameters)
            {
                if (p.Value == null)
                {
                    p.Value = DBNull.Value;
                }
            }
        }
    }
}