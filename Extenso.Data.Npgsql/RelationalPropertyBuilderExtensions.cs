using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Extenso.Data.Npgsql;

public static class RelationalPropertyBuilderExtensions
{
    extension(PropertyBuilder propertyBuilder)
    {
        public PropertyBuilder ForNpgsqlHasColumnType(string typeName) => propertyBuilder.HasAnnotation("Npgsql:ColumnType", typeName);
    }
}