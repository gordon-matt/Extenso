using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Extenso.AspNetCore.Mvc.ViewFeatures;

/// <summary>
/// Provides extension methods for template infos.
/// </summary>
public static class TemplateInfoExtensions
{
    extension(TemplateInfo templateInfo)
    {
        /// <summary>
        /// Gets the full HTML field identifier.
        /// </summary>
        /// <param name="partialFieldName">The partial field name.</param>
        /// <returns>The full HTML field identifier.</returns>
        public static string GetFullHtmlFieldId(string partialFieldName) =>
            TagBuilder.CreateSanitizedId(partialFieldName, "-");
    }
}