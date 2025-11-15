using System.ComponentModel;

namespace Extenso.KendoGridBinder.Extensions;

[Browsable(false)]
[EditorBrowsable(EditorBrowsableState.Never)]
internal static class QueryProviderExtensions
{
    extension(IQueryProvider provider)
    {
        public bool IsQueryTranslatorProvider() =>
            provider.GetType().FullName.Contains("QueryInterceptor.QueryTranslatorProvider");

        public bool IsEntityFrameworkProvider() =>
            provider.GetType().FullName == "System.Data.Objects.ELinq.ObjectQueryProvider" ||
            provider.GetType().FullName.StartsWith("System.Data.Entity.Internal.Linq");

        public bool IsLinqToObjectsProvider() =>
            provider.GetType().FullName.Contains("EnumerableQuery");
    }
}