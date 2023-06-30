using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;

namespace Extenso.Data.Common
{
    internal static class SqlDbTypeConverter
    {
        private static readonly Lazy<HashSet<(SqlDbType, Type)>> netTypes;
        private static readonly Lazy<HashSet<(SqlDbType, DbType)>> dbTypes;

        static SqlDbTypeConverter()
        {
            netTypes = new Lazy<HashSet<(SqlDbType, Type)>>(() =>
            {
                return new HashSet<(SqlDbType, Type)>
                {
                    (SqlDbType.BigInt, typeof(long)),
                    (SqlDbType.Binary, typeof(byte[])),
                    (SqlDbType.Bit, typeof(bool)),
                    (SqlDbType.Char, typeof(string)),
                    (SqlDbType.Date, typeof(DateOnly)),
                    (SqlDbType.DateTime, typeof(DateTime)),
                    (SqlDbType.DateTime2, typeof(DateTime)),
                    (SqlDbType.DateTimeOffset, typeof(DateTimeOffset)),
                    (SqlDbType.Decimal, typeof(decimal)),
                    (SqlDbType.Float, typeof(double)),
                    (SqlDbType.Int, typeof(int)),
                    (SqlDbType.Money, typeof(decimal)),
                    (SqlDbType.NChar, typeof(string)),
                    (SqlDbType.NText, typeof(string)),
                    (SqlDbType.NVarChar, typeof(string)),
                    (SqlDbType.Real, typeof(float)),
                    (SqlDbType.SmallDateTime, typeof(DateTime)),
                    (SqlDbType.SmallInt, typeof(short)),
                    (SqlDbType.SmallMoney, typeof(decimal)),
                    (SqlDbType.Structured, typeof(object)),
                    (SqlDbType.Text, typeof(string)),
                    (SqlDbType.Time, typeof(TimeSpan)),
                    (SqlDbType.Timestamp, typeof(byte[])),
                    (SqlDbType.TinyInt, typeof(byte)),
                    (SqlDbType.Udt, typeof(object)),
                    (SqlDbType.UniqueIdentifier, typeof(Guid)),
                    (SqlDbType.VarBinary, typeof(byte[])),
                    (SqlDbType.VarChar, typeof(string)),
                    (SqlDbType.Variant, typeof(object)),
                    (SqlDbType.Xml, typeof(SqlXml))
                };
            });
            dbTypes = new Lazy<HashSet<(SqlDbType, DbType)>>(() =>
            {
                return new HashSet<(SqlDbType, DbType)>
                {
                    (SqlDbType.BigInt, DbType.Int64),
                    (SqlDbType.Binary, DbType.Binary),
                    (SqlDbType.Bit, DbType.Boolean),
                    (SqlDbType.Char, DbType.StringFixedLength),
                    (SqlDbType.Date, DbType.Date),
                    (SqlDbType.DateTime, DbType.DateTime),
                    (SqlDbType.DateTime2, DbType.DateTime2),
                    (SqlDbType.DateTimeOffset, DbType.DateTimeOffset),
                    (SqlDbType.Decimal, DbType.Decimal),
                    (SqlDbType.Float, DbType.Double),
                    (SqlDbType.Int, DbType.Int32),
                    (SqlDbType.Money, DbType.Currency),
                    (SqlDbType.NChar, DbType.String),
                    (SqlDbType.NText, DbType.String),
                    (SqlDbType.NVarChar, DbType.String),
                    (SqlDbType.Real, DbType.Single),
                    (SqlDbType.SmallDateTime, DbType.DateTime),
                    (SqlDbType.SmallInt, DbType.Int16),
                    (SqlDbType.SmallMoney, DbType.Currency),
                    (SqlDbType.Structured, DbType.Object),
                    (SqlDbType.Text, DbType.String),
                    (SqlDbType.Time, DbType.Time),
                    (SqlDbType.Timestamp, DbType.Binary),
                    (SqlDbType.TinyInt, DbType.Byte),
                    (SqlDbType.Udt, DbType.Object),
                    (SqlDbType.UniqueIdentifier, DbType.Guid),
                    (SqlDbType.VarBinary, DbType.Binary),
                    (SqlDbType.VarChar, DbType.String),
                    (SqlDbType.Variant, DbType.Object),
                    (SqlDbType.Xml, DbType.Xml)
                };
            });
        }

        public static Type ToSystemType(SqlDbType sqlDbType) => netTypes.Value.First(x => x.Item1 == sqlDbType).Item2;

        public static DbType ToDbType(SqlDbType sqlDbType) => dbTypes.Value.First(x => x.Item1 == sqlDbType).Item2;
    }
}