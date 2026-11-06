using Microsoft.AspNetCore.Mvc;

// ReSharper disable once CheckNamespace
namespace Extenso.KendoGridBinder.ModelBinder.Mvc;

/// <summary>
/// Represents the kendo grid MVC request.
/// </summary>
[ModelBinder(BinderType = typeof(KendoGridMvcModelBinder))]
public class KendoGridMvcRequest : KendoGridBaseRequest
{
}