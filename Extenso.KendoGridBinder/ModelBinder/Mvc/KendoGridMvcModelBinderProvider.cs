using Microsoft.AspNetCore.Mvc.ModelBinding;

// ReSharper disable once CheckNamespace
namespace Extenso.KendoGridBinder.ModelBinder.Mvc;

/// <summary>
/// Represents the kendo grid MVC model binder provider.
/// </summary>
public class KendoGridMvcModelBinderProvider : IModelBinderProvider
{
    /// <summary>
    /// Gets the binder.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <returns>The binder.</returns>
    public IModelBinder GetBinder(ModelBinderProviderContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        return context.Metadata.ModelType == typeof(KendoGridMvcRequest) ? new KendoGridMvcModelBinder() : (IModelBinder)null;
    }
}