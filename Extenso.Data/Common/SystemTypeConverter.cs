using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Extenso.Data.Common;

internal static class SystemTypeConverter
{
    private static readonly Lazy<HashSet<(Type, DbType)>> dbTypes;
    private static readonly Lazy<HashSet<(Type, SqlDbType)>> sqlDbTypes;

    static SystemTypeConverter()
    {
        dbTypes = new Lazy<HashSet<(Type, DbType)>>(() =>
        {
            return
            [
                (typeof(bool), DbType.Boolean),
                (typeof(byte), DbType.Byte),
                (typeof(char), DbType.StringFixedLength),
                (typeof(short), DbType.Int16),
                (typeof(int), DbType.Int32),
                (typeof(long), DbType.Int64),
                (typeof(decimal), DbType.Decimal),
                (typeof(double), DbType.Double),
                (typeof(DateOnly), DbType.Date),
                (typeof(DateTime), DbType.DateTime),
                (typeof(DateTimeOffset), DbType.DateTimeOffset),
                (typeof(Guid), DbType.Guid),
                (typeof(float), DbType.Single),
                (typeof(string), DbType.String),
                (typeof(sbyte), DbType.SByte),
                (typeof(TimeSpan), DbType.Time),
                (typeof(ushort), DbType.UInt16),
                (typeof(uint), DbType.UInt32),
                (typeof(ulong), DbType.UInt64),
                (typeof(Uri), DbType.String)
            ];
        });
        sqlDbTypes = new Lazy<HashSet<(Type, SqlDbType)>>(() =>
        {
            return
            [
                (typeof(bool), SqlDbType.Bit),
                (typeof(byte), SqlDbType.TinyInt),
                (typeof(char), SqlDbType.NChar),
                (typeof(short), SqlDbType.SmallInt),
                (typeof(int), SqlDbType.Int),
                (typeof(long), SqlDbType.BigInt),
                (typeof(decimal), SqlDbType.Decimal),
                (typeof(double), SqlDbType.Float),
                (typeof(DateOnly), SqlDbType.Date),
                (typeof(DateTime), SqlDbType.DateTime),
                (typeof(DateTimeOffset), SqlDbType.DateTimeOffset),
                (typeof(Guid), SqlDbType.UniqueIdentifier),
                (typeof(float), SqlDbType.Real),
                (typeof(string), SqlDbType.NVarChar),
                (typeof(sbyte), SqlDbType.TinyInt),
                (typeof(TimeSpan), SqlDbType.Time),
                (typeof(ushort), SqlDbType.SmallInt),
                (typeof(uint), SqlDbType.Int),
                (typeof(ulong), SqlDbType.BigInt),
                (typeof(Uri), SqlDbType.NVarChar)
            ];
        });
    }

    public static DbType ToDbType(Type systemType) => dbTypes.Value.First(x => x.Item1 == systemType).Item2;

    public static SqlDbType ToSqlDbType(Type systemType) => sqlDbTypes.Value.First(x => x.Item1 == systemType).Item2;
}