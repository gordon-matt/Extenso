using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;

namespace Extenso.AspNetCore.Mvc.Html;

/// <summary>
/// Provides extension methods for HTML contents.
/// </summary>
public static class HtmlContentExtensions
{
    extension(IHtmlContent htmlContent)
    {
        /// <summary>
        /// Gets the string.
        /// </summary>
        /// <returns>The string.</returns>
        public string GetString()
        {
            using var stringWriter = new StringWriter();
            htmlContent.WriteTo(stringWriter, HtmlEncoder.Default);
            return stringWriter.ToString();
        }
    }
}