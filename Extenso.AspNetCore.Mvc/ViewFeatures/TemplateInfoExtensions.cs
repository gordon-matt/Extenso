using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Extenso.AspNetCore.Mvc.ViewFeatures;

public static class TemplateInfoExtensions
{
    extension(TemplateInfo templateInfo)
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Extension method.")]
        public string GetFullHtmlFieldId(string partialFieldName) =>
            TagBuilder.CreateSanitizedId(partialFieldName, "-");
    }
}