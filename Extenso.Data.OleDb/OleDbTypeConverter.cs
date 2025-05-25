using System.Data;
using System.Data.OleDb;

namespace Extenso.Data.OleDb;

public static class OleDbTypeConverter
{
    private static readonly Lazy<HashSet<(OleDbType OleDbType, Type SystemType)>> netTypes;
    private static readonly Lazy<HashSet<(OleDbType OleDbType, SqlDbType SqlDbType)>> sqlDbTypes;
    private static readonly Lazy<HashSet<(OleDbType OleDbType, DbType DbType)>> dbTypes;

    static OleDbTypeConverter()
    {
        netTypes = new Lazy<HashSet<(OleDbType, Type)>>(() => [
            (OleDbType.BigInt, typeof(long)),
            (OleDbType.Binary, typeof(byte[])),
            (OleDbType.Boolean, typeof(bool)),
            (OleDbType.BSTR, typeof(string)),
            (OleDbType.Char, typeof(string)),
            (OleDbType.Currency, typeof(decimal)),
            (OleDbType.Date, typeof(DateOnly)),
            (OleDbType.DBDate, typeof(DateTime)),
            (OleDbType.DBTime, typeof(TimeOnly)),
            (OleDbType.DBTimeStamp, typeof(DateTime)),
            (OleDbType.Decimal, typeof(decimal)),
            (OleDbType.Double, typeof(double)),
            (OleDbType.Empty, null),
            (OleDbType.Error, typeof(Exception)),
            (OleDbType.Filetime, typeof(DateTime)),
            (OleDbType.Guid, typeof(Guid)),
            (OleDbType.IDispatch, typeof(object)),
            (OleDbType.Integer, typeof(int)),
            (OleDbType.IUnknown, typeof(object)),
            (OleDbType.LongVarBinary, typeof(byte[])),
            (OleDbType.LongVarChar, typeof(string)),
            (OleDbType.LongVarWChar, typeof(string)),
            (OleDbType.Numeric, typeof(decimal)),
            (OleDbType.PropVariant, typeof(object)),
            (OleDbType.Single, typeof(float)),
            (OleDbType.SmallInt, typeof(short)),
            (OleDbType.TinyInt, typeof(sbyte)),
            (OleDbType.UnsignedBigInt, typeof(ulong)),
            (OleDbType.UnsignedInt, typeof(uint)),
            (OleDbType.UnsignedSmallInt, typeof(ushort)),
            (OleDbType.UnsignedTinyInt, typeof(byte)),
            (OleDbType.VarBinary, typeof(byte[])),
            (OleDbType.VarChar, typeof(string)),
            (OleDbType.Variant, typeof(object)),
            (OleDbType.VarNumeric, typeof(decimal)),
            (OleDbType.VarWChar, typeof(string)),
            (OleDbType.WChar, typeof(string))
        ]);
        sqlDbTypes = new Lazy<HashSet<(OleDbType, SqlDbType)>>(() => [
            (OleDbType.BigInt, SqlDbType.BigInt),
            (OleDbType.Binary, SqlDbType.Binary),
            (OleDbType.Boolean, SqlDbType.Bit),
            (OleDbType.BSTR, SqlDbType.NVarChar),
            (OleDbType.Char, SqlDbType.VarChar),
            (OleDbType.Currency, SqlDbType.Money),
            (OleDbType.Date, SqlDbType.Date),
            (OleDbType.DBDate, SqlDbType.Date),
            (OleDbType.DBTime, SqlDbType.Time),
            (OleDbType.DBTimeStamp, SqlDbType.DateTime),
            (OleDbType.Decimal, SqlDbType.Decimal),
            (OleDbType.Double, SqlDbType.Float),
            (OleDbType.Empty, SqlDbType.Variant),//correct?
            (OleDbType.Error, SqlDbType.Variant),//correct?
            (OleDbType.Filetime, SqlDbType.VarChar),
            (OleDbType.Guid, SqlDbType.UniqueIdentifier),
            (OleDbType.IDispatch, SqlDbType.Variant),
            (OleDbType.Integer, SqlDbType.Int),
            (OleDbType.IUnknown, SqlDbType.Variant),
            (OleDbType.LongVarBinary, SqlDbType.VarBinary),
            (OleDbType.LongVarChar, SqlDbType.VarChar),
            (OleDbType.LongVarWChar, SqlDbType.NVarChar),
            (OleDbType.Numeric, SqlDbType.Decimal),
            (OleDbType.PropVariant, SqlDbType.Variant),
            (OleDbType.Single, SqlDbType.Float),
            (OleDbType.SmallInt, SqlDbType.SmallInt),
            (OleDbType.TinyInt, SqlDbType.TinyInt),//cannot map exactly
            (OleDbType.UnsignedBigInt, SqlDbType.BigInt),//cannot map exactly
            (OleDbType.UnsignedInt, SqlDbType.Int),//cannot map exactly
            (OleDbType.UnsignedSmallInt, SqlDbType.SmallInt),//cannot map exactly
            (OleDbType.UnsignedTinyInt, SqlDbType.TinyInt),
            (OleDbType.VarBinary, SqlDbType.VarBinary),
            (OleDbType.VarChar, SqlDbType.VarChar),
            (OleDbType.Variant, SqlDbType.Variant),
            (OleDbType.VarNumeric, SqlDbType.Decimal),
            (OleDbType.VarWChar, SqlDbType.NVarChar),
            (OleDbType.WChar, SqlDbType.NVarChar)
        ]);
        dbTypes = new Lazy<HashSet<(OleDbType, DbType)>>(() => [
            (OleDbType.BigInt, DbType.Int64),
            (OleDbType.Binary, DbType.Binary),
            (OleDbType.Boolean, DbType.Boolean),
            (OleDbType.BSTR, DbType.String),
            (OleDbType.Char, DbType.String),
            (OleDbType.Currency, DbType.Currency),
            (OleDbType.Date, DbType.Date),
            (OleDbType.DBDate, DbType.Date),
            (OleDbType.DBTime, DbType.Time),
            (OleDbType.DBTimeStamp, DbType.DateTime),
            (OleDbType.Decimal, DbType.Decimal),
            (OleDbType.Double, DbType.Double),
            (OleDbType.Empty, DbType.Object),//correct?
            (OleDbType.Error, DbType.Object),//correct?
            (OleDbType.Filetime, DbType.UInt64),
            (OleDbType.Guid, DbType.Guid),
            (OleDbType.IDispatch, DbType.Object),
            (OleDbType.Integer, DbType.Int32),
            (OleDbType.IUnknown, DbType.Object),
            (OleDbType.LongVarBinary, DbType.Binary),
            (OleDbType.LongVarChar, DbType.String),
            (OleDbType.LongVarWChar, DbType.String),
            (OleDbType.Numeric, DbType.Decimal),
            (OleDbType.PropVariant, DbType.Object),
            (OleDbType.Single, DbType.Single),
            (OleDbType.SmallInt, DbType.Int16),
            (OleDbType.TinyInt, DbType.SByte),
            (OleDbType.UnsignedBigInt, DbType.UInt64),
            (OleDbType.UnsignedInt, DbType.UInt32),
            (OleDbType.UnsignedSmallInt, DbType.UInt16),
            (OleDbType.UnsignedTinyInt, DbType.Byte),
            (OleDbType.VarBinary, DbType.Binary),
            (OleDbType.VarChar, DbType.String),
            (OleDbType.Variant, DbType.Object),
            (OleDbType.VarNumeric, DbType.VarNumeric),
            (OleDbType.VarWChar, DbType.String),
            (OleDbType.WChar, DbType.String)
        ]);
    }

    public static Type ToSystemType(OleDbType oleDbType) => netTypes.Value.First(x => x.OleDbType == oleDbType).SystemType;

    public static DbType ToDbType(OleDbType oleDbType) => dbTypes.Value.First(x => x.OleDbType == oleDbType).DbType;

    public static SqlDbType ToSqlDbType(OleDbType oleDbType) => sqlDbTypes.Value.First(x => x.OleDbType == oleDbType).SqlDbType;
}