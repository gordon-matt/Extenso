using System;
using System.Data;
using System.Linq;
using Extenso.Collections.Generic;

namespace Extenso.Data.Common
{
    internal static class SystemTypeConverter
    {
        private static readonly Lazy<TupleHashSet<Type, DbType>> dbTypes;
        private static readonly Lazy<TupleHashSet<Type, SqlDbType>> sqlDbTypes;

        static SystemTypeConverter()
        {
            dbTypes = new Lazy<TupleHashSet<Type, DbType>>(() =>
            {
                return new TupleHashSet<Type, DbType>
                {
                    { typeof(Boolean), DbType.Boolean },
                    { typeof(Byte), DbType.Byte },
                    { typeof(Char), DbType.StringFixedLength },
                    { typeof(Int16), DbType.Int16 },
                    { typeof(Int32), DbType.Int32 },
                    { typeof(Int64), DbType.Int64 },
                    { typeof(Decimal), DbType.Decimal },
                    { typeof(Double), DbType.Double },
                    { typeof(DateTime), DbType.DateTime },
                    { typeof(DateTimeOffset), DbType.DateTimeOffset },
                    { typeof(Guid), DbType.Guid },
                    { typeof(Single), DbType.Single },
                    { typeof(String), DbType.String },
                    { typeof(SByte), DbType.SByte },
                    { typeof(TimeSpan), DbType.Time },
                    { typeof(UInt16), DbType.UInt16 },
                    { typeof(UInt32), DbType.UInt32 },
                    { typeof(UInt64), DbType.UInt64 },
                    { typeof(Uri), DbType.String }
                };
            });
            sqlDbTypes = new Lazy<TupleHashSet<Type, SqlDbType>>(() =>
            {
                return new TupleHashSet<Type, SqlDbType>
                {
                    { typeof(Boolean), SqlDbType.Bit },
                    { typeof(Byte), SqlDbType.TinyInt },
                    { typeof(Char), SqlDbType.NChar },
                    { typeof(Int16), SqlDbType.SmallInt },
                    { typeof(Int32), SqlDbType.Int },
                    { typeof(Int64), SqlDbType.BigInt },
                    { typeof(Decimal), SqlDbType.Decimal },
                    { typeof(Double), SqlDbType.Float },
                    { typeof(DateTime), SqlDbType.DateTime },
                    { typeof(DateTimeOffset), SqlDbType.DateTimeOffset },
                    { typeof(Guid), SqlDbType.UniqueIdentifier },
                    { typeof(Single), SqlDbType.Real },
                    { typeof(String), SqlDbType.NVarChar },
                    { typeof(SByte), SqlDbType.TinyInt },
                    { typeof(TimeSpan), SqlDbType.Time },
                    { typeof(UInt16), SqlDbType.SmallInt },
                    { typeof(UInt32), SqlDbType.Int },
                    { typeof(UInt64), SqlDbType.BigInt },
                    { typeof(Uri), SqlDbType.NVarChar }
                };
            });
        }

        public static DbType ToDbType(Type systemType)
        {
            return dbTypes.Value.First(x => x.Item1 == systemType).Item2;
        }

        public static SqlDbType ToSqlDbType(Type systemType)
        {
            return sqlDbTypes.Value.First(x => x.Item1 == systemType).Item2;
        }
    }
}