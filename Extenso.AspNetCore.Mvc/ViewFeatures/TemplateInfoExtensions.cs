using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Extenso.AspNetCore.Mvc.ViewFeatures;

public static class TemplateInfoExtensions
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Extension method.")]
    public static string GetFullHtmlFieldId(this TemplateInfo templateInfo, string partialFieldName) => TagBuilder.CreateSanitizedId(partialFieldName, "-");
}