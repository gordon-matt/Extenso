using System;
using System.Data;
//using System.Data.OleDb;
using System.Data.SqlTypes;
using System.Linq;
using Extenso.Collections.Generic;

namespace Extenso.Data.Common
{
    internal static class SqlDbTypeConverter
    {
        private static readonly Lazy<TupleHashSet<SqlDbType, Type>> netTypes;
        private static readonly Lazy<TupleHashSet<SqlDbType, DbType>> dbTypes;

        static SqlDbTypeConverter()
        {
            netTypes = new Lazy<TupleHashSet<SqlDbType, Type>>(() =>
            {
                var hashSet = new TupleHashSet<SqlDbType, Type>();
                hashSet.Add(SqlDbType.BigInt, typeof(Int64));
                hashSet.Add(SqlDbType.Binary, typeof(Byte[]));
                hashSet.Add(SqlDbType.Bit, typeof(Boolean));
                hashSet.Add(SqlDbType.Char, typeof(String));
                hashSet.Add(SqlDbType.Date, typeof(DateTime));
                hashSet.Add(SqlDbType.DateTime, typeof(DateTime));
                hashSet.Add(SqlDbType.DateTime2, typeof(DateTime));
                hashSet.Add(SqlDbType.DateTimeOffset, typeof(DateTimeOffset));
                hashSet.Add(SqlDbType.Decimal, typeof(Decimal));
                hashSet.Add(SqlDbType.Float, typeof(Double));
                hashSet.Add(SqlDbType.Int, typeof(Int32));
                hashSet.Add(SqlDbType.Money, typeof(Decimal));
                hashSet.Add(SqlDbType.NChar, typeof(String));
                hashSet.Add(SqlDbType.NText, typeof(String));
                hashSet.Add(SqlDbType.NVarChar, typeof(String));
                hashSet.Add(SqlDbType.Real, typeof(Single));
                hashSet.Add(SqlDbType.SmallDateTime, typeof(DateTime));
                hashSet.Add(SqlDbType.SmallInt, typeof(Int16));
                hashSet.Add(SqlDbType.SmallMoney, typeof(Decimal));
                hashSet.Add(SqlDbType.Structured, typeof(Object));
                hashSet.Add(SqlDbType.Text, typeof(String));
                hashSet.Add(SqlDbType.Time, typeof(TimeSpan));
                hashSet.Add(SqlDbType.Timestamp, typeof(Byte[]));
                hashSet.Add(SqlDbType.TinyInt, typeof(Byte));
                hashSet.Add(SqlDbType.Udt, typeof(Object));
                hashSet.Add(SqlDbType.UniqueIdentifier, typeof(Guid));
                hashSet.Add(SqlDbType.VarBinary, typeof(Byte[]));
                hashSet.Add(SqlDbType.VarChar, typeof(String));
                hashSet.Add(SqlDbType.Variant, typeof(Object));
                hashSet.Add(SqlDbType.Xml, typeof(SqlXml));
                return hashSet;
            });
            dbTypes = new Lazy<TupleHashSet<SqlDbType, DbType>>(() =>
            {
                var hashSet = new TupleHashSet<SqlDbType, DbType>();
                hashSet.Add(SqlDbType.BigInt, DbType.Int64);
                hashSet.Add(SqlDbType.Binary, DbType.Binary);
                hashSet.Add(SqlDbType.Bit, DbType.Boolean);
                hashSet.Add(SqlDbType.Char, DbType.StringFixedLength);
                hashSet.Add(SqlDbType.Date, DbType.Date);
                hashSet.Add(SqlDbType.DateTime, DbType.DateTime);
                hashSet.Add(SqlDbType.DateTime2, DbType.DateTime2);
                hashSet.Add(SqlDbType.DateTimeOffset, DbType.DateTimeOffset);
                hashSet.Add(SqlDbType.Decimal, DbType.Decimal);
                hashSet.Add(SqlDbType.Float, DbType.Double);
                hashSet.Add(SqlDbType.Int, DbType.Int32);
                hashSet.Add(SqlDbType.Money, DbType.Currency);
                hashSet.Add(SqlDbType.NChar, DbType.String);
                hashSet.Add(SqlDbType.NText, DbType.String);
                hashSet.Add(SqlDbType.NVarChar, DbType.String);
                hashSet.Add(SqlDbType.Real, DbType.Single);
                hashSet.Add(SqlDbType.SmallDateTime, DbType.DateTime);
                hashSet.Add(SqlDbType.SmallInt, DbType.Int16);
                hashSet.Add(SqlDbType.SmallMoney, DbType.Currency);
                hashSet.Add(SqlDbType.Structured, DbType.Object);
                hashSet.Add(SqlDbType.Text, DbType.String);
                hashSet.Add(SqlDbType.Time, DbType.Time);
                hashSet.Add(SqlDbType.Timestamp, DbType.Binary);
                hashSet.Add(SqlDbType.TinyInt, DbType.Byte);
                hashSet.Add(SqlDbType.Udt, DbType.Object);
                hashSet.Add(SqlDbType.UniqueIdentifier, DbType.Guid);
                hashSet.Add(SqlDbType.VarBinary, DbType.Binary);
                hashSet.Add(SqlDbType.VarChar, DbType.String);
                hashSet.Add(SqlDbType.Variant, DbType.Object);
                hashSet.Add(SqlDbType.Xml, DbType.Xml);
                return hashSet;
            });
        }

        public static Type ToSystemType(SqlDbType sqlDbType)
        {
            return netTypes.Value.First(x => x.Item1 == sqlDbType).Item2;
        }

        public static DbType ToDbType(SqlDbType sqlDbType)
        {
            return dbTypes.Value.First(x => x.Item1 == sqlDbType).Item2;
        }
    }
}