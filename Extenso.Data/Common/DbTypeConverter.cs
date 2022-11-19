using System;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using Extenso.Collections.Generic;

namespace Extenso.Data.Common
{
    internal static class DbTypeConverter
    {
        private static readonly Lazy<TupleHashSet<DbType, Type>> netTypes;
        private static readonly Lazy<TupleHashSet<DbType, SqlDbType>> sqlDbTypes;

        static DbTypeConverter()
        {
            netTypes = new Lazy<TupleHashSet<DbType, Type>>(() =>
            {
                return new TupleHashSet<DbType, Type>
                {
                    { DbType.AnsiString, typeof(String) },
                    { DbType.AnsiStringFixedLength, typeof(Char) },
                    { DbType.Binary, typeof(Byte[]) },
                    { DbType.Boolean, typeof(Boolean) },
                    { DbType.Byte, typeof(Byte) },
                    { DbType.Currency, typeof(Decimal) },
                    { DbType.Date, typeof(DateTime) },
                    { DbType.DateTime, typeof(DateTime) },
                    { DbType.DateTime2, typeof(DateTime) },
                    { DbType.DateTimeOffset, typeof(DateTimeOffset) },
                    { DbType.Decimal, typeof(Decimal) },
                    { DbType.Double, typeof(Double) },
                    { DbType.Guid, typeof(Guid) },
                    { DbType.Int16, typeof(Int16) },
                    { DbType.Int32, typeof(Int32) },
                    { DbType.Int64, typeof(Int64) },
                    { DbType.Object, typeof(Object) },
                    { DbType.SByte, typeof(SByte) },
                    { DbType.Single, typeof(Single) },
                    { DbType.String, typeof(String) },
                    { DbType.StringFixedLength, typeof(Char) },
                    { DbType.Time, typeof(TimeSpan) },
                    { DbType.UInt16, typeof(UInt16) },
                    { DbType.UInt32, typeof(UInt32) },
                    { DbType.UInt64, typeof(UInt64) },
                    { DbType.VarNumeric, typeof(Int32) },
                    { DbType.Xml, typeof(SqlXml) }
                };
            });
            sqlDbTypes = new Lazy<TupleHashSet<DbType, SqlDbType>>(() =>
            {
                return new TupleHashSet<DbType, SqlDbType>
                {
                    { DbType.AnsiString, SqlDbType.VarChar },
                    { DbType.AnsiStringFixedLength, SqlDbType.Char },
                    { DbType.Binary, SqlDbType.Binary },
                    { DbType.Boolean, SqlDbType.Bit },
                    { DbType.Byte, SqlDbType.TinyInt },
                    { DbType.Currency, SqlDbType.Money },
                    { DbType.Date, SqlDbType.Date },
                    { DbType.DateTime, SqlDbType.DateTime },
                    { DbType.DateTime2, SqlDbType.DateTime2 },
                    { DbType.DateTimeOffset, SqlDbType.DateTimeOffset },
                    { DbType.Decimal, SqlDbType.Decimal },
                    { DbType.Double, SqlDbType.Float },
                    { DbType.Guid, SqlDbType.UniqueIdentifier },
                    { DbType.Int16, SqlDbType.SmallInt },
                    { DbType.Int32, SqlDbType.Int },
                    { DbType.Int64, SqlDbType.BigInt },
                    { DbType.Object, SqlDbType.Variant },
                    { DbType.SByte, SqlDbType.TinyInt },
                    { DbType.Single, SqlDbType.Real },
                    { DbType.String, SqlDbType.NVarChar },
                    { DbType.StringFixedLength, SqlDbType.NChar },
                    { DbType.Time, SqlDbType.Time },
                    { DbType.UInt16, SqlDbType.SmallInt },
                    { DbType.UInt32, SqlDbType.Int },
                    { DbType.UInt64, SqlDbType.BigInt },
                    { DbType.VarNumeric, SqlDbType.Int },
                    { DbType.Xml, SqlDbType.Xml }
                };
            });
        }

        public static Type ToSystemType(DbType dbType)
        {
            return netTypes.Value.First(x => x.Item1 == dbType).Item2;
        }

        public static SqlDbType ToSqlDbType(DbType dbType)
        {
            return sqlDbTypes.Value.First(x => x.Item1 == dbType).Item2;
        }
    }
}