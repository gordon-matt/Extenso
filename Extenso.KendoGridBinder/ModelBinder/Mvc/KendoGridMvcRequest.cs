using Microsoft.AspNetCore.Mvc;

// ReSharper disable once CheckNamespace
namespace Extenso.KendoGridBinder.ModelBinder.Mvc;

[ModelBinder(BinderType = typeof(KendoGridMvcModelBinder))]
public class KendoGridMvcRequest : KendoGridBaseRequest
{
}