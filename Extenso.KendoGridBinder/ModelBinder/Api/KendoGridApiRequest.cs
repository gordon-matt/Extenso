using Microsoft.AspNetCore.Mvc;

// ReSharper disable once CheckNamespace
namespace Extenso.KendoGridBinder.ModelBinder.Api;

[ModelBinder(BinderType = typeof(KendoGridApiModelBinder))]
public class KendoGridApiRequest : KendoGridBaseRequest
{
}