using System;
using System.Data;
using System.Linq;
using Extenso.Collections.Generic;

namespace Extenso.Data.Common
{
    internal static class SystemTypeConverter
    {
        private static Lazy<TupleHashSet<Type, DbType>> dbTypes;
        private static Lazy<TupleHashSet<Type, SqlDbType>> sqlDbTypes;

        static SystemTypeConverter()
        {
            dbTypes = new Lazy<TupleHashSet<Type, DbType>>(() =>
            {
                var hashSet = new TupleHashSet<Type, DbType>();
                hashSet.Add(typeof(Boolean), DbType.Boolean);
                hashSet.Add(typeof(Byte), DbType.Byte);
                hashSet.Add(typeof(Char), DbType.StringFixedLength);
                hashSet.Add(typeof(Int16), DbType.Int16);
                hashSet.Add(typeof(Int32), DbType.Int32);
                hashSet.Add(typeof(Int64), DbType.Int64);
                hashSet.Add(typeof(Decimal), DbType.Decimal);
                hashSet.Add(typeof(Double), DbType.Double);
                hashSet.Add(typeof(DateTime), DbType.DateTime);
                hashSet.Add(typeof(DateTimeOffset), DbType.DateTimeOffset);
                hashSet.Add(typeof(Guid), DbType.Guid);
                hashSet.Add(typeof(Single), DbType.Single);
                hashSet.Add(typeof(String), DbType.String);
                hashSet.Add(typeof(SByte), DbType.SByte);
                hashSet.Add(typeof(TimeSpan), DbType.Time);
                hashSet.Add(typeof(UInt16), DbType.UInt16);
                hashSet.Add(typeof(UInt32), DbType.UInt32);
                hashSet.Add(typeof(UInt64), DbType.UInt64);
                hashSet.Add(typeof(Uri), DbType.String);
                return hashSet;
            });
            sqlDbTypes = new Lazy<TupleHashSet<Type, SqlDbType>>(() =>
            {
                var hashSet = new TupleHashSet<Type, SqlDbType>();
                hashSet.Add(typeof(Boolean), SqlDbType.Bit);
                hashSet.Add(typeof(Byte), SqlDbType.TinyInt);
                hashSet.Add(typeof(Char), SqlDbType.NChar);
                hashSet.Add(typeof(Int16), SqlDbType.SmallInt);
                hashSet.Add(typeof(Int32), SqlDbType.Int);
                hashSet.Add(typeof(Int64), SqlDbType.BigInt);
                hashSet.Add(typeof(Decimal), SqlDbType.Decimal);
                hashSet.Add(typeof(Double), SqlDbType.Float);
                hashSet.Add(typeof(DateTime), SqlDbType.DateTime);
                hashSet.Add(typeof(DateTimeOffset), SqlDbType.DateTimeOffset);
                hashSet.Add(typeof(Guid), SqlDbType.UniqueIdentifier);
                hashSet.Add(typeof(Single), SqlDbType.Real);
                hashSet.Add(typeof(String), SqlDbType.NVarChar);
                hashSet.Add(typeof(SByte), SqlDbType.TinyInt);
                hashSet.Add(typeof(TimeSpan), SqlDbType.Time);
                hashSet.Add(typeof(UInt16), SqlDbType.SmallInt);
                hashSet.Add(typeof(UInt32), SqlDbType.Int);
                hashSet.Add(typeof(UInt64), SqlDbType.BigInt);
                hashSet.Add(typeof(Uri), SqlDbType.NVarChar);
                return hashSet;
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