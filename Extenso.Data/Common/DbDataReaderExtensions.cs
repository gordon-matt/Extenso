using System.Data.Common;

namespace Extenso.Data.Common;

public static class DbDataReaderExtensions
{
    extension(DbDataReader reader)
    {
        public bool? GetBooleanNullable(int ordinal)
        {
            object value = reader.GetValue(ordinal);
            return value is null || value == DBNull.Value ? null : Convert.ToBoolean(value);
        }

        public byte? GetByteNullable(int ordinal)
        {
            object value = reader.GetValue(ordinal);
            return value is null || value == DBNull.Value ? null : Convert.ToByte(value);
        }

        public char? GetCharNullable(int ordinal)
        {
            object value = reader.GetValue(ordinal);
            return value is null || value == DBNull.Value ? null : Convert.ToChar(value);
        }

        public DateTime? GetDateTimeNullable(int ordinal)
        {
            object value = reader.GetValue(ordinal);
            return value is null || value == DBNull.Value ? null : value.ConvertTo<DateTime>();
        }

        public decimal? GetDecimalNullable(int ordinal)
        {
            object value = reader.GetValue(ordinal);
            return value is null || value == DBNull.Value ? null : Convert.ToDecimal(value);
        }

        public double? GetDoubleNullable(int ordinal)
        {
            object value = reader.GetValue(ordinal);
            return value is null || value == DBNull.Value ? null : Convert.ToDouble(value);
        }

        public float? GetFloatNullable(int ordinal)
        {
            object value = reader.GetValue(ordinal);
            return value is null || value == DBNull.Value ? null : Convert.ToSingle(value);
        }

        public Guid? GetGuidNullable(int ordinal)
        {
            object value = reader.GetValue(ordinal);
            return value is null || value == DBNull.Value ? null : new Guid(value.ToString());
        }

        public short? GetInt16Nullable(int ordinal)
        {
            object value = reader.GetValue(ordinal);
            return value is null || value == DBNull.Value ? null : Convert.ToInt16(value);
        }

        public int? GetInt32Nullable(int ordinal)
        {
            object value = reader.GetValue(ordinal);
            return value is null || value == DBNull.Value ? null : Convert.ToInt32(value);
        }

        public long? GetInt64Nullable(int ordinal)
        {
            object value = reader.GetValue(ordinal);
            return value is null || value == DBNull.Value ? null : Convert.ToInt64(value);
        }
    }
}