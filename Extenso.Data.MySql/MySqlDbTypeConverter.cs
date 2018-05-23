using System;
using System.Data;
using System.Linq;
using Extenso.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Extenso.Data.MySql
{
    internal static class MySqlDbTypeConverter
    {
        private static Lazy<TupleHashSet<MySqlDbType, Type>> netTypes;
        private static Lazy<TupleHashSet<MySqlDbType, DbType>> dbTypes;
        private static Lazy<TupleHashSet<MySqlDbType, SqlDbType>> sqlDbTypes;

        static MySqlDbTypeConverter()
        {
            netTypes = new Lazy<TupleHashSet<MySqlDbType, Type>>(() =>
            {
                var hashSet = new TupleHashSet<MySqlDbType, Type>();
                hashSet.Add(MySqlDbType.Binary, typeof(byte[]));
                hashSet.Add(MySqlDbType.Bit, typeof(long)); // If length is 1, then should be "bool"
                hashSet.Add(MySqlDbType.Blob, typeof(object));
                hashSet.Add(MySqlDbType.Byte, typeof(sbyte));
                hashSet.Add(MySqlDbType.Date, typeof(DateTime));
                hashSet.Add(MySqlDbType.Datetime, typeof(DateTime));
                hashSet.Add(MySqlDbType.DateTime, typeof(DateTime));
                hashSet.Add(MySqlDbType.Decimal, typeof(decimal));
                hashSet.Add(MySqlDbType.Double, typeof(double));
                hashSet.Add(MySqlDbType.Enum, typeof(object));
                hashSet.Add(MySqlDbType.Float, typeof(float));
                //hashSet.Add(MySqlDbType.Geometry, typeof(DbGeometry));
                hashSet.Add(MySqlDbType.Guid, typeof(Guid));
                hashSet.Add(MySqlDbType.Int16, typeof(short));
                hashSet.Add(MySqlDbType.Int24, typeof(int));
                hashSet.Add(MySqlDbType.Int32, typeof(int));
                hashSet.Add(MySqlDbType.Int64, typeof(long));
                hashSet.Add(MySqlDbType.JSON, typeof(string));
                hashSet.Add(MySqlDbType.LongBlob, typeof(object));
                hashSet.Add(MySqlDbType.LongText, typeof(string));
                hashSet.Add(MySqlDbType.MediumBlob, typeof(object));
                hashSet.Add(MySqlDbType.MediumText, typeof(string));
                hashSet.Add(MySqlDbType.Newdate, typeof(DateTime));
                hashSet.Add(MySqlDbType.NewDecimal, typeof(decimal));
                hashSet.Add(MySqlDbType.Set, typeof(object));
                hashSet.Add(MySqlDbType.String, typeof(string));
                hashSet.Add(MySqlDbType.Text, typeof(string));
                hashSet.Add(MySqlDbType.Time, typeof(TimeSpan));
                hashSet.Add(MySqlDbType.Timestamp, typeof(DateTime));
                hashSet.Add(MySqlDbType.TinyBlob, typeof(object));
                hashSet.Add(MySqlDbType.TinyText, typeof(string));
                hashSet.Add(MySqlDbType.UByte, typeof(byte));
                hashSet.Add(MySqlDbType.UInt16, typeof(ushort));
                hashSet.Add(MySqlDbType.UInt24, typeof(uint));
                hashSet.Add(MySqlDbType.UInt32, typeof(uint));
                hashSet.Add(MySqlDbType.UInt64, typeof(ulong));
                hashSet.Add(MySqlDbType.VarBinary, typeof(byte[]));
                hashSet.Add(MySqlDbType.VarChar, typeof(string));
                hashSet.Add(MySqlDbType.VarString, typeof(string));
                hashSet.Add(MySqlDbType.Year, typeof(short));
                return hashSet;
            });
            dbTypes = new Lazy<TupleHashSet<MySqlDbType, DbType>>(() =>
            {
                var hashSet = new TupleHashSet<MySqlDbType, DbType>();
                hashSet.Add(MySqlDbType.Binary, DbType.Binary);
                hashSet.Add(MySqlDbType.Bit, DbType.Int64); // If length is 1, then should be "bool"
                hashSet.Add(MySqlDbType.Blob, DbType.Object);
                hashSet.Add(MySqlDbType.Byte, DbType.SByte);
                hashSet.Add(MySqlDbType.Date, DbType.DateTime);
                hashSet.Add(MySqlDbType.Datetime, DbType.DateTime);
                hashSet.Add(MySqlDbType.DateTime, DbType.DateTime);
                hashSet.Add(MySqlDbType.Decimal, DbType.Decimal);
                hashSet.Add(MySqlDbType.Double, DbType.Double);
                hashSet.Add(MySqlDbType.Enum, DbType.Object);
                hashSet.Add(MySqlDbType.Float, DbType.Single);
                hashSet.Add(MySqlDbType.Geometry, DbType.Object);
                hashSet.Add(MySqlDbType.Guid, DbType.Guid);
                hashSet.Add(MySqlDbType.Int16, DbType.Int16);
                hashSet.Add(MySqlDbType.Int24, DbType.Int32);
                hashSet.Add(MySqlDbType.Int32, DbType.Int32);
                hashSet.Add(MySqlDbType.Int64, DbType.Int64);
                hashSet.Add(MySqlDbType.JSON, DbType.String);
                hashSet.Add(MySqlDbType.LongBlob, DbType.Object);
                hashSet.Add(MySqlDbType.LongText, DbType.String);
                hashSet.Add(MySqlDbType.MediumBlob, DbType.Object);
                hashSet.Add(MySqlDbType.MediumText, DbType.String);
                hashSet.Add(MySqlDbType.Newdate, DbType.DateTime);
                hashSet.Add(MySqlDbType.NewDecimal, DbType.Decimal);
                hashSet.Add(MySqlDbType.Set, DbType.Object);
                hashSet.Add(MySqlDbType.String, DbType.String);
                hashSet.Add(MySqlDbType.Text, DbType.String);
                hashSet.Add(MySqlDbType.Time, DbType.Time);
                hashSet.Add(MySqlDbType.Timestamp, DbType.DateTime);
                hashSet.Add(MySqlDbType.TinyBlob, DbType.Object);
                hashSet.Add(MySqlDbType.TinyText, DbType.String);
                hashSet.Add(MySqlDbType.UByte, DbType.Byte);
                hashSet.Add(MySqlDbType.UInt16, DbType.UInt16);
                hashSet.Add(MySqlDbType.UInt24, DbType.UInt32);
                hashSet.Add(MySqlDbType.UInt32, DbType.UInt32);
                hashSet.Add(MySqlDbType.UInt64, DbType.UInt64);
                hashSet.Add(MySqlDbType.VarBinary, DbType.Binary);
                hashSet.Add(MySqlDbType.VarChar, DbType.String);
                hashSet.Add(MySqlDbType.VarString, DbType.String);
                hashSet.Add(MySqlDbType.Year, DbType.Int16);
                return hashSet;
            });
            sqlDbTypes = new Lazy<TupleHashSet<MySqlDbType, SqlDbType>>(() =>
            {
                var hashSet = new TupleHashSet<MySqlDbType, SqlDbType>();
                hashSet.Add(MySqlDbType.Binary, SqlDbType.Binary);
                hashSet.Add(MySqlDbType.Bit, SqlDbType.BigInt); // If length is 1, then should be "bool"
                hashSet.Add(MySqlDbType.Blob, SqlDbType.Binary);
                hashSet.Add(MySqlDbType.Byte, SqlDbType.TinyInt);
                hashSet.Add(MySqlDbType.Date, SqlDbType.Date);
                hashSet.Add(MySqlDbType.Datetime, SqlDbType.DateTime);
                hashSet.Add(MySqlDbType.DateTime, SqlDbType.DateTime);
                hashSet.Add(MySqlDbType.Decimal, SqlDbType.Decimal);
                hashSet.Add(MySqlDbType.Double, SqlDbType.Float);
                hashSet.Add(MySqlDbType.Enum, SqlDbType.Variant);
                hashSet.Add(MySqlDbType.Float, SqlDbType.Real);
                hashSet.Add(MySqlDbType.Geometry, SqlDbType.Variant);
                hashSet.Add(MySqlDbType.Guid, SqlDbType.UniqueIdentifier);
                hashSet.Add(MySqlDbType.Int16, SqlDbType.SmallInt);
                hashSet.Add(MySqlDbType.Int24, SqlDbType.Int);
                hashSet.Add(MySqlDbType.Int32, SqlDbType.Int);
                hashSet.Add(MySqlDbType.Int64, SqlDbType.BigInt);
                hashSet.Add(MySqlDbType.JSON, SqlDbType.NVarChar);
                hashSet.Add(MySqlDbType.LongBlob, SqlDbType.Variant);
                hashSet.Add(MySqlDbType.LongText, SqlDbType.NVarChar);
                hashSet.Add(MySqlDbType.MediumBlob, SqlDbType.Variant);
                hashSet.Add(MySqlDbType.MediumText, SqlDbType.NVarChar);
                hashSet.Add(MySqlDbType.Newdate, SqlDbType.DateTime);
                hashSet.Add(MySqlDbType.NewDecimal, SqlDbType.Decimal);
                hashSet.Add(MySqlDbType.Set, SqlDbType.Variant);
                hashSet.Add(MySqlDbType.String, SqlDbType.NVarChar);
                hashSet.Add(MySqlDbType.Text, SqlDbType.NVarChar);
                hashSet.Add(MySqlDbType.Time, SqlDbType.Time);
                hashSet.Add(MySqlDbType.Timestamp, SqlDbType.Timestamp);
                hashSet.Add(MySqlDbType.TinyBlob, SqlDbType.Variant);
                hashSet.Add(MySqlDbType.TinyText, SqlDbType.NVarChar);
                hashSet.Add(MySqlDbType.UByte, SqlDbType.TinyInt);
                hashSet.Add(MySqlDbType.UInt16, SqlDbType.SmallInt);
                hashSet.Add(MySqlDbType.UInt24, SqlDbType.Int);
                hashSet.Add(MySqlDbType.UInt32, SqlDbType.Int);
                hashSet.Add(MySqlDbType.UInt64, SqlDbType.BigInt);
                hashSet.Add(MySqlDbType.VarBinary, SqlDbType.Binary);
                hashSet.Add(MySqlDbType.VarChar, SqlDbType.NVarChar);
                hashSet.Add(MySqlDbType.VarString, SqlDbType.NVarChar);
                hashSet.Add(MySqlDbType.Year, SqlDbType.SmallInt);
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