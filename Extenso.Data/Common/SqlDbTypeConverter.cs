using System;
using System.Data;
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
                return new TupleHashSet<SqlDbType, Type>
                {
                    { SqlDbType.BigInt, typeof(Int64) },
                    { SqlDbType.Binary, typeof(Byte[]) },
                    { SqlDbType.Bit, typeof(Boolean) },
                    { SqlDbType.Char, typeof(String) },
                    { SqlDbType.Date, typeof(DateTime) },
                    { SqlDbType.DateTime, typeof(DateTime) },
                    { SqlDbType.DateTime2, typeof(DateTime) },
                    { SqlDbType.DateTimeOffset, typeof(DateTimeOffset) },
                    { SqlDbType.Decimal, typeof(Decimal) },
                    { SqlDbType.Float, typeof(Double) },
                    { SqlDbType.Int, typeof(Int32) },
                    { SqlDbType.Money, typeof(Decimal) },
                    { SqlDbType.NChar, typeof(String) },
                    { SqlDbType.NText, typeof(String) },
                    { SqlDbType.NVarChar, typeof(String) },
                    { SqlDbType.Real, typeof(Single) },
                    { SqlDbType.SmallDateTime, typeof(DateTime) },
                    { SqlDbType.SmallInt, typeof(Int16) },
                    { SqlDbType.SmallMoney, typeof(Decimal) },
                    { SqlDbType.Structured, typeof(Object) },
                    { SqlDbType.Text, typeof(String) },
                    { SqlDbType.Time, typeof(TimeSpan) },
                    { SqlDbType.Timestamp, typeof(Byte[]) },
                    { SqlDbType.TinyInt, typeof(Byte) },
                    { SqlDbType.Udt, typeof(Object) },
                    { SqlDbType.UniqueIdentifier, typeof(Guid) },
                    { SqlDbType.VarBinary, typeof(Byte[]) },
                    { SqlDbType.VarChar, typeof(String) },
                    { SqlDbType.Variant, typeof(Object) },
                    { SqlDbType.Xml, typeof(SqlXml) }
                };
            });
            dbTypes = new Lazy<TupleHashSet<SqlDbType, DbType>>(() =>
            {
                return new TupleHashSet<SqlDbType, DbType>
                {
                    { SqlDbType.BigInt, DbType.Int64 },
                    { SqlDbType.Binary, DbType.Binary },
                    { SqlDbType.Bit, DbType.Boolean },
                    { SqlDbType.Char, DbType.StringFixedLength },
                    { SqlDbType.Date, DbType.Date },
                    { SqlDbType.DateTime, DbType.DateTime },
                    { SqlDbType.DateTime2, DbType.DateTime2 },
                    { SqlDbType.DateTimeOffset, DbType.DateTimeOffset },
                    { SqlDbType.Decimal, DbType.Decimal },
                    { SqlDbType.Float, DbType.Double },
                    { SqlDbType.Int, DbType.Int32 },
                    { SqlDbType.Money, DbType.Currency },
                    { SqlDbType.NChar, DbType.String },
                    { SqlDbType.NText, DbType.String },
                    { SqlDbType.NVarChar, DbType.String },
                    { SqlDbType.Real, DbType.Single },
                    { SqlDbType.SmallDateTime, DbType.DateTime },
                    { SqlDbType.SmallInt, DbType.Int16 },
                    { SqlDbType.SmallMoney, DbType.Currency },
                    { SqlDbType.Structured, DbType.Object },
                    { SqlDbType.Text, DbType.String },
                    { SqlDbType.Time, DbType.Time },
                    { SqlDbType.Timestamp, DbType.Binary },
                    { SqlDbType.TinyInt, DbType.Byte },
                    { SqlDbType.Udt, DbType.Object },
                    { SqlDbType.UniqueIdentifier, DbType.Guid },
                    { SqlDbType.VarBinary, DbType.Binary },
                    { SqlDbType.VarChar, DbType.String },
                    { SqlDbType.Variant, DbType.Object },
                    { SqlDbType.Xml, DbType.Xml }
                };
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