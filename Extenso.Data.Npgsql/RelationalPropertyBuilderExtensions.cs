using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Extenso.Data.Npgsql;

public static class RelationalPropertyBuilderExtensions
{
    public static PropertyBuilder ForNpgsqlHasColumnType(this PropertyBuilder propertyBuilder, string typeName) => propertyBuilder.HasAnnotation("Npgsql:ColumnType", typeName);
}