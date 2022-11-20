using System;
using System.Data;
using System.Linq;
using Extenso.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Extenso.Data.MySql
{
    internal static class MySqlDbTypeConverter
    {
        private static readonly Lazy<TupleHashSet<MySqlDbType, Type>> netTypes;
        private static readonly Lazy<TupleHashSet<MySqlDbType, DbType>> dbTypes;
        private static readonly Lazy<TupleHashSet<MySqlDbType, SqlDbType>> sqlDbTypes;

        static MySqlDbTypeConverter()
        {
            netTypes = new Lazy<TupleHashSet<MySqlDbType, Type>>(() =>
            {
                return new TupleHashSet<MySqlDbType, Type>
                {
                    { MySqlDbType.Binary, typeof(byte[]) },
                    { MySqlDbType.Bit, typeof(long) }, // If length is 1, then should be "bool"
                    { MySqlDbType.Blob, typeof(object) },
                    { MySqlDbType.Byte, typeof(sbyte) },
                    { MySqlDbType.Date, typeof(DateTime) },
                    { MySqlDbType.Datetime, typeof(DateTime) },
                    { MySqlDbType.DateTime, typeof(DateTime) },
                    { MySqlDbType.Decimal, typeof(decimal) },
                    { MySqlDbType.Double, typeof(double) },
                    { MySqlDbType.Enum, typeof(object) },
                    { MySqlDbType.Float, typeof(float) },
                    //hashSet.Add(MySqlDbType.Geometry, typeof(DbGeometry));
                    { MySqlDbType.Guid, typeof(Guid) },
                    { MySqlDbType.Int16, typeof(short) },
                    { MySqlDbType.Int24, typeof(int) },
                    { MySqlDbType.Int32, typeof(int) },
                    { MySqlDbType.Int64, typeof(long) },
                    { MySqlDbType.JSON, typeof(string) },
                    { MySqlDbType.LongBlob, typeof(object) },
                    { MySqlDbType.LongText, typeof(string) },
                    { MySqlDbType.MediumBlob, typeof(object) },
                    { MySqlDbType.MediumText, typeof(string) },
                    { MySqlDbType.Newdate, typeof(DateTime) },
                    { MySqlDbType.NewDecimal, typeof(decimal) },
                    { MySqlDbType.Set, typeof(object) },
                    { MySqlDbType.String, typeof(string) },
                    { MySqlDbType.Text, typeof(string) },
                    { MySqlDbType.Time, typeof(TimeSpan) },
                    { MySqlDbType.Timestamp, typeof(DateTime) },
                    { MySqlDbType.TinyBlob, typeof(object) },
                    { MySqlDbType.TinyText, typeof(string) },
                    { MySqlDbType.UByte, typeof(byte) },
                    { MySqlDbType.UInt16, typeof(ushort) },
                    { MySqlDbType.UInt24, typeof(uint) },
                    { MySqlDbType.UInt32, typeof(uint) },
                    { MySqlDbType.UInt64, typeof(ulong) },
                    { MySqlDbType.VarBinary, typeof(byte[]) },
                    { MySqlDbType.VarChar, typeof(string) },
                    { MySqlDbType.VarString, typeof(string) },
                    { MySqlDbType.Year, typeof(short) }
                };
            });
            dbTypes = new Lazy<TupleHashSet<MySqlDbType, DbType>>(() =>
            {
                return new TupleHashSet<MySqlDbType, DbType>
                {
                    { MySqlDbType.Binary, DbType.Binary },
                    { MySqlDbType.Bit, DbType.Int64 }, // If length is 1, then should be "bool"
                    { MySqlDbType.Blob, DbType.Object },
                    { MySqlDbType.Byte, DbType.SByte },
                    { MySqlDbType.Date, DbType.DateTime },
                    { MySqlDbType.Datetime, DbType.DateTime },
                    { MySqlDbType.DateTime, DbType.DateTime },
                    { MySqlDbType.Decimal, DbType.Decimal },
                    { MySqlDbType.Double, DbType.Double },
                    { MySqlDbType.Enum, DbType.Object },
                    { MySqlDbType.Float, DbType.Single },
                    { MySqlDbType.Geometry, DbType.Object },
                    { MySqlDbType.Guid, DbType.Guid },
                    { MySqlDbType.Int16, DbType.Int16 },
                    { MySqlDbType.Int24, DbType.Int32 },
                    { MySqlDbType.Int32, DbType.Int32 },
                    { MySqlDbType.Int64, DbType.Int64 },
                    { MySqlDbType.JSON, DbType.String },
                    { MySqlDbType.LongBlob, DbType.Object },
                    { MySqlDbType.LongText, DbType.String },
                    { MySqlDbType.MediumBlob, DbType.Object },
                    { MySqlDbType.MediumText, DbType.String },
                    { MySqlDbType.Newdate, DbType.DateTime },
                    { MySqlDbType.NewDecimal, DbType.Decimal },
                    { MySqlDbType.Set, DbType.Object },
                    { MySqlDbType.String, DbType.String },
                    { MySqlDbType.Text, DbType.String },
                    { MySqlDbType.Time, DbType.Time },
                    { MySqlDbType.Timestamp, DbType.DateTime },
                    { MySqlDbType.TinyBlob, DbType.Object },
                    { MySqlDbType.TinyText, DbType.String },
                    { MySqlDbType.UByte, DbType.Byte },
                    { MySqlDbType.UInt16, DbType.UInt16 },
                    { MySqlDbType.UInt24, DbType.UInt32 },
                    { MySqlDbType.UInt32, DbType.UInt32 },
                    { MySqlDbType.UInt64, DbType.UInt64 },
                    { MySqlDbType.VarBinary, DbType.Binary },
                    { MySqlDbType.VarChar, DbType.String },
                    { MySqlDbType.VarString, DbType.String },
                    { MySqlDbType.Year, DbType.Int16 }
                };
            });
            sqlDbTypes = new Lazy<TupleHashSet<MySqlDbType, SqlDbType>>(() =>
            {
                var hashSet = new TupleHashSet<MySqlDbType, SqlDbType>
                {
                    { MySqlDbType.Binary, SqlDbType.Binary },
                    { MySqlDbType.Bit, SqlDbType.BigInt }, // If length is 1, then should be "bool"
                    { MySqlDbType.Blob, SqlDbType.Binary },
                    { MySqlDbType.Byte, SqlDbType.TinyInt },
                    { MySqlDbType.Date, SqlDbType.Date },
                    { MySqlDbType.Datetime, SqlDbType.DateTime },
                    { MySqlDbType.DateTime, SqlDbType.DateTime },
                    { MySqlDbType.Decimal, SqlDbType.Decimal },
                    { MySqlDbType.Double, SqlDbType.Float },
                    { MySqlDbType.Enum, SqlDbType.Variant },
                    { MySqlDbType.Float, SqlDbType.Real },
                    { MySqlDbType.Geometry, SqlDbType.Variant },
                    { MySqlDbType.Guid, SqlDbType.UniqueIdentifier },
                    { MySqlDbType.Int16, SqlDbType.SmallInt },
                    { MySqlDbType.Int24, SqlDbType.Int },
                    { MySqlDbType.Int32, SqlDbType.Int },
                    { MySqlDbType.Int64, SqlDbType.BigInt },
                    { MySqlDbType.JSON, SqlDbType.NVarChar },
                    { MySqlDbType.LongBlob, SqlDbType.Variant },
                    { MySqlDbType.LongText, SqlDbType.NVarChar },
                    { MySqlDbType.MediumBlob, SqlDbType.Variant },
                    { MySqlDbType.MediumText, SqlDbType.NVarChar },
                    { MySqlDbType.Newdate, SqlDbType.DateTime },
                    { MySqlDbType.NewDecimal, SqlDbType.Decimal },
                    { MySqlDbType.Set, SqlDbType.Variant },
                    { MySqlDbType.String, SqlDbType.NVarChar },
                    { MySqlDbType.Text, SqlDbType.NVarChar },
                    { MySqlDbType.Time, SqlDbType.Time },
                    { MySqlDbType.Timestamp, SqlDbType.Timestamp },
                    { MySqlDbType.TinyBlob, SqlDbType.Variant },
                    { MySqlDbType.TinyText, SqlDbType.NVarChar },
                    { MySqlDbType.UByte, SqlDbType.TinyInt },
                    { MySqlDbType.UInt16, SqlDbType.SmallInt },
                    { MySqlDbType.UInt24, SqlDbType.Int },
                    { MySqlDbType.UInt32, SqlDbType.Int },
                    { MySqlDbType.UInt64, SqlDbType.BigInt },
                    { MySqlDbType.VarBinary, SqlDbType.Binary },
                    { MySqlDbType.VarChar, SqlDbType.NVarChar },
                    { MySqlDbType.VarString, SqlDbType.NVarChar },
                    { MySqlDbType.Year, SqlDbType.SmallInt }
                };
                return hashSet;
            });
        }

        public static Type ToSystemType(MySqlDbType mySqlDbType)
        {
            return netTypes.Value.First(x => x.Item1 == mySqlDbType).Item2;
        }

        public static DbType ToDbType(MySqlDbType mySqlDbType)
        {
            return dbTypes.Value.First(x => x.Item1 == mySqlDbType).Item2;
        }

        public static SqlDbType ToSqlDbType(MySqlDbType mySqlDbType)
        {
            return sqlDbTypes.Value.First(x => x.Item1 == mySqlDbType).Item2;
        }
    }
}