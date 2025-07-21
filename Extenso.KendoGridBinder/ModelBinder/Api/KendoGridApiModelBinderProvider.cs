using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

// ReSharper disable once CheckNamespace
namespace Extenso.KendoGridBinder.ModelBinder.Api;

public class KendoGridApiModelBinderProvider : IModelBinderProvider
{
    public IModelBinder GetBinder(ModelBinderProviderContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        return context.Metadata.ModelType == typeof(KendoGridApiRequest) ? new BinderTypeModelBinder(typeof(KendoGridApiModelBinder)) : (IModelBinder)null;
    }
}