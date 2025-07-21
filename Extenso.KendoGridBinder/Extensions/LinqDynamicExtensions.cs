using System.ComponentModel;
using System.Reflection;

namespace Extenso.KendoGridBinder.Extensions;

[Browsable(false)]
[EditorBrowsable(EditorBrowsableState.Never)]
internal static class DynamicQueryableExtensions
{
    public static IEnumerable<TEntity> Select<TEntity>(this IEnumerable<object> source, string propertyName) => source.Select(x => GetPropertyValue<TEntity>(x, propertyName));

    private static T GetPropertyValue<T>(object self, string propertyName)
    {
        var type = self.GetType();
        var propInfo = type.GetTypeInfo().GetProperty(propertyName);

        try
        {
            return propInfo != null ? (T)propInfo.GetValue(self, null) : default;
        }
        catch
        {
            return default;
        }
    }
}