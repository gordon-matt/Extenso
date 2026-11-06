using System.Data.Common;

namespace Extenso.Data.Common;

/// <summary>
/// Provides extension methods for database data readers.
/// </summary>
public static class DbDataReaderExtensions
{
    extension(DbDataReader reader)
    {
        /// <summary>
        /// Gets the boolean nullable.
        /// </summary>
        /// <param name="ordinal">The ordinal.</param>
        /// <returns>true when the condition is met; otherwise, false.</returns>
        public bool? GetBooleanNullable(int ordinal)
        {
            object value = reader.GetValue(ordinal);
            return value is null || value == DBNull.Value ? null : Convert.ToBoolean(value);
        }

        /// <summary>
        /// Gets the byte nullable.
        /// </summary>
        /// <param name="ordinal">The ordinal.</param>
        /// <returns>The byte nullable.</returns>
        public byte? GetByteNullable(int ordinal)
        {
            object value = reader.GetValue(ordinal);
            return value is null || value == DBNull.Value ? null : Convert.ToByte(value);
        }

        /// <summary>
        /// Gets the char nullable.
        /// </summary>
        /// <param name="ordinal">The ordinal.</param>
        /// <returns>The char nullable.</returns>
        public char? GetCharNullable(int ordinal)
        {
            object value = reader.GetValue(ordinal);
            return value is null || value == DBNull.Value ? null : Convert.ToChar(value);
        }

        /// <summary>
        /// Gets the date time nullable.
        /// </summary>
        /// <param name="ordinal">The ordinal.</param>
        /// <returns>The date time nullable.</returns>
        public DateTime? GetDateTimeNullable(int ordinal)
        {
            object value = reader.GetValue(ordinal);
            return value is null || value == DBNull.Value ? null : value.ConvertTo<DateTime>();
        }

        /// <summary>
        /// Gets the decimal nullable.
        /// </summary>
        /// <param name="ordinal">The ordinal.</param>
        /// <returns>The decimal nullable.</returns>
        public decimal? GetDecimalNullable(int ordinal)
        {
            object value = reader.GetValue(ordinal);
            return value is null || value == DBNull.Value ? null : Convert.ToDecimal(value);
        }

        /// <summary>
        /// Gets the double nullable.
        /// </summary>
        /// <param name="ordinal">The ordinal.</param>
        /// <returns>The double nullable.</returns>
        public double? GetDoubleNullable(int ordinal)
        {
            object value = reader.GetValue(ordinal);
            return value is null || value == DBNull.Value ? null : Convert.ToDouble(value);
        }

        /// <summary>
        /// Gets the float nullable.
        /// </summary>
        /// <param name="ordinal">The ordinal.</param>
        /// <returns>The float nullable.</returns>
        public float? GetFloatNullable(int ordinal)
        {
            object value = reader.GetValue(ordinal);
            return value is null || value == DBNull.Value ? null : Convert.ToSingle(value);
        }

        /// <summary>
        /// Gets the GUID nullable.
        /// </summary>
        /// <param name="ordinal">The ordinal.</param>
        /// <returns>The GUID nullable.</returns>
        public Guid? GetGuidNullable(int ordinal)
        {
            object value = reader.GetValue(ordinal);
            return value is null || value == DBNull.Value ? null : new Guid(value.ToString());
        }

        /// <summary>
        /// Gets the int16nullable.
        /// </summary>
        /// <param name="ordinal">The ordinal.</param>
        /// <returns>The int16nullable.</returns>
        public short? GetInt16Nullable(int ordinal)
        {
            object value = reader.GetValue(ordinal);
            return value is null || value == DBNull.Value ? null : Convert.ToInt16(value);
        }

        /// <summary>
        /// Gets the int32nullable.
        /// </summary>
        /// <param name="ordinal">The ordinal.</param>
        /// <returns>The int32nullable.</returns>
        public int? GetInt32Nullable(int ordinal)
        {
            object value = reader.GetValue(ordinal);
            return value is null || value == DBNull.Value ? null : Convert.ToInt32(value);
        }

        /// <summary>
        /// Gets the int64nullable.
        /// </summary>
        /// <param name="ordinal">The ordinal.</param>
        /// <returns>The int64nullable.</returns>
        public long? GetInt64Nullable(int ordinal)
        {
            object value = reader.GetValue(ordinal);
            return value is null || value == DBNull.Value ? null : Convert.ToInt64(value);
        }
    }
}