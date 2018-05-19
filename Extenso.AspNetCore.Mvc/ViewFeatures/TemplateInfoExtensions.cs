using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Extenso.AspNetCore.Mvc.ViewFeatures
{
    public static class TemplateInfoExtensions
    {
        public static string GetFullHtmlFieldId(this TemplateInfo templateInfo, string partialFieldName)
        {
            return TagBuilder.CreateSanitizedId(partialFieldName, "-");
        }
    }
}