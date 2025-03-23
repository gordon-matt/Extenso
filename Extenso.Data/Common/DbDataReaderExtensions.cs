using System;
using System.Data.Common;

namespace Extenso.Data.Common;

public static class DbDataReaderExtensions
{
    public static bool? GetBooleanNullable(this DbDataReader reader, int ordinal)
    {
        object value = reader.GetValue(ordinal);
        return value == null || value == DBNull.Value ? null : Convert.ToBoolean(value);
    }

    public static byte? GetByteNullable(this DbDataReader reader, int ordinal)
    {
        object value = reader.GetValue(ordinal);
        return value == null || value == DBNull.Value ? null : Convert.ToByte(value);
    }

    public static char? GetCharNullable(this DbDataReader reader, int ordinal)
    {
        object value = reader.GetValue(ordinal);
        return value == null || value == DBNull.Value ? null : Convert.ToChar(value);
    }

    public static DateTime? GetDateTimeNullable(this DbDataReader reader, int ordinal)
    {
        object value = reader.GetValue(ordinal);
        return value == null || value == DBNull.Value ? null : value.ConvertTo<DateTime>();
    }

    public static decimal? GetDecimalNullable(this DbDataReader reader, int ordinal)
    {
        object value = reader.GetValue(ordinal);
        return value == null || value == DBNull.Value ? null : Convert.ToDecimal(value);
    }

    public static double? GetDoubleNullable(this DbDataReader reader, int ordinal)
    {
        object value = reader.GetValue(ordinal);
        return value == null || value == DBNull.Value ? null : Convert.ToDouble(value);
    }

    public static float? GetFloatNullable(this DbDataReader reader, int ordinal)
    {
        object value = reader.GetValue(ordinal);
        return value == null || value == DBNull.Value ? null : Convert.ToSingle(value);
    }

    public static Guid? GetGuidNullable(this DbDataReader reader, int ordinal)
    {
        object value = reader.GetValue(ordinal);
        return value == null || value == DBNull.Value ? null : new Guid(value.ToString());
    }

    public static short? GetInt16Nullable(this DbDataReader reader, int ordinal)
    {
        object value = reader.GetValue(ordinal);
        return value == null || value == DBNull.Value ? null : Convert.ToInt16(value);
    }

    public static int? GetInt32Nullable(this DbDataReader reader, int ordinal)
    {
        object value = reader.GetValue(ordinal);
        return value == null || value == DBNull.Value ? null : Convert.ToInt32(value);
    }

    public static long? GetInt64Nullable(this DbDataReader reader, int ordinal)
    {
        object value = reader.GetValue(ordinal);
        return value == null || value == DBNull.Value ? null : Convert.ToInt64(value);
    }
}