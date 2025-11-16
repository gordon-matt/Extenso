using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Extenso.AspNetCore.Mvc.ViewFeatures;

public static class TemplateInfoExtensions
{
    extension(TemplateInfo templateInfo)
    {
        public static string GetFullHtmlFieldId(string partialFieldName) =>
            TagBuilder.CreateSanitizedId(partialFieldName, "-");
    }
}