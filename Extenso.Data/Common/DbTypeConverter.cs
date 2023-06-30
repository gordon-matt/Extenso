using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;

namespace Extenso.Data.Common
{
    internal static class DbTypeConverter
    {
        private static readonly Lazy<HashSet<(DbType, Type)>> netTypes;
        private static readonly Lazy<HashSet<(DbType, SqlDbType)>> sqlDbTypes;

        static DbTypeConverter()
        {
            netTypes = new Lazy<HashSet<(DbType, Type)>>(() =>
            {
                return new HashSet<(DbType, Type)>
                {
                    (DbType.AnsiString, typeof(string)),
                    (DbType.AnsiStringFixedLength, typeof(char)),
                    (DbType.Binary, typeof(byte[])),
                    (DbType.Boolean, typeof(bool)),
                    (DbType.Byte, typeof(byte)),
                    (DbType.Currency, typeof(decimal)),
                    (DbType.Date, typeof(DateOnly)),
                    (DbType.DateTime, typeof(DateTime)),
                    (DbType.DateTime2, typeof(DateTime)),
                    (DbType.DateTimeOffset, typeof(DateTimeOffset)),
                    (DbType.Decimal, typeof(decimal)),
                    (DbType.Double, typeof(double)),
                    (DbType.Guid, typeof(Guid)),
                    (DbType.Int16, typeof(short)),
                    (DbType.Int32, typeof(int)),
                    (DbType.Int64, typeof(long)),
                    (DbType.Object, typeof(object)),
                    (DbType.SByte, typeof(sbyte)),
                    (DbType.Single, typeof(float)),
                    (DbType.String, typeof(string)),
                    (DbType.StringFixedLength, typeof(char)),
                    (DbType.Time, typeof(TimeSpan)),
                    (DbType.UInt16, typeof(ushort)),
                    (DbType.UInt32, typeof(uint)),
                    (DbType.UInt64, typeof(ulong)),
                    (DbType.VarNumeric, typeof(int)),
                    (DbType.Xml, typeof(SqlXml))
                };
            });
            sqlDbTypes = new Lazy<HashSet<(DbType, SqlDbType)>>(() =>
            {
                return new HashSet<(DbType, SqlDbType)>
                {
                    (DbType.AnsiString, SqlDbType.VarChar),
                    (DbType.AnsiStringFixedLength, SqlDbType.Char),
                    (DbType.Binary, SqlDbType.Binary),
                    (DbType.Boolean, SqlDbType.Bit),
                    (DbType.Byte, SqlDbType.TinyInt),
                    (DbType.Currency, SqlDbType.Money),
                    (DbType.Date, SqlDbType.Date),
                    (DbType.DateTime, SqlDbType.DateTime),
                    (DbType.DateTime2, SqlDbType.DateTime2),
                    (DbType.DateTimeOffset, SqlDbType.DateTimeOffset),
                    (DbType.Decimal, SqlDbType.Decimal),
                    (DbType.Double, SqlDbType.Float),
                    (DbType.Guid, SqlDbType.UniqueIdentifier),
                    (DbType.Int16, SqlDbType.SmallInt),
                    (DbType.Int32, SqlDbType.Int),
                    (DbType.Int64, SqlDbType.BigInt),
                    (DbType.Object, SqlDbType.Variant),
                    (DbType.SByte, SqlDbType.TinyInt),
                    (DbType.Single, SqlDbType.Real),
                    (DbType.String, SqlDbType.NVarChar),
                    (DbType.StringFixedLength, SqlDbType.NChar),
                    (DbType.Time, SqlDbType.Time),
                    (DbType.UInt16, SqlDbType.SmallInt),
                    (DbType.UInt32, SqlDbType.Int),
                    (DbType.UInt64, SqlDbType.BigInt),
                    (DbType.VarNumeric, SqlDbType.Int),
                    (DbType.Xml, SqlDbType.Xml)
                };
            });
        }

        public static Type ToSystemType(DbType dbType) => netTypes.Value.First(x => x.Item1 == dbType).Item2;

        public static SqlDbType ToSqlDbType(DbType dbType) => sqlDbTypes.Value.First(x => x.Item1 == dbType).Item2;
    }
}