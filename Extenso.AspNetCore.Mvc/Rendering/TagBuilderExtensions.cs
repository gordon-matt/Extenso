using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Extenso.AspNetCore.Mvc.Rendering;

/// <summary>
/// Provides extension methods for tag builders.
/// </summary>
public static class TagBuilderExtensions
{
    extension(TagBuilder tagBuilder)
    {
        /// <summary>
        /// Builds the value.
        /// </summary>
        /// <returns>The string.</returns>
        public string Build()
        {
            using var stringWriter = new StringWriter();
            tagBuilder.WriteTo(stringWriter, HtmlEncoder.Default);
            return stringWriter.ToString();
        }
    }
}