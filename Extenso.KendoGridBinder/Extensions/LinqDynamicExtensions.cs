using System.ComponentModel;
using System.Reflection;

namespace Extenso.KendoGridBinder.Extensions;

[Browsable(false)]
[EditorBrowsable(EditorBrowsableState.Never)]
internal static class DynamicQueryableExtensions
{
    extension(IEnumerable<object> source)
    {
        public IEnumerable<TEntity> Select<TEntity>(string propertyName) =>
            source.Select(x => GetPropertyValue<TEntity>(x, propertyName));
    }

    private static T GetPropertyValue<T>(this object self, string propertyName)
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