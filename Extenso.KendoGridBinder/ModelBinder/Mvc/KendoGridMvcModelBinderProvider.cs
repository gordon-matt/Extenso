using Microsoft.AspNetCore.Mvc.ModelBinding;

// ReSharper disable once CheckNamespace
namespace Extenso.KendoGridBinder.ModelBinder.Mvc;

public class KendoGridMvcModelBinderProvider : IModelBinderProvider
{
    public IModelBinder GetBinder(ModelBinderProviderContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        return context.Metadata.ModelType == typeof(KendoGridMvcRequest) ? new KendoGridMvcModelBinder() : (IModelBinder)null;
    }
}