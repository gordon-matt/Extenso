using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Extenso.Data.Npgsql;

/// <summary>
/// Provides extension methods for relational property builders.
/// </summary>
public static class RelationalPropertyBuilderExtensions
{
    extension(PropertyBuilder propertyBuilder)
    {
        /// <summary>
        /// Gets the for npgsql has column type.
        /// </summary>
        /// <param name="typeName">The type name.</param>
        /// <returns>The for npgsql has column type.</returns>
        public PropertyBuilder ForNpgsqlHasColumnType(string typeName) => propertyBuilder.HasAnnotation("Npgsql:ColumnType", typeName);
    }
}