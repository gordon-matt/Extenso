using Microsoft.AspNetCore.Mvc;

// ReSharper disable once CheckNamespace
namespace Extenso.KendoGridBinder.ModelBinder.Api;

/// <summary>
/// Represents the kendo grid API request.
/// </summary>
[ModelBinder(BinderType = typeof(KendoGridApiModelBinder))]
public class KendoGridApiRequest : KendoGridBaseRequest
{
}