using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;

namespace Extenso.AspNetCore.Mvc.Html;

public static class HtmlContentExtensions
{
    extension(IHtmlContent htmlContent)
    {
        public string GetString()
        {
            using var stringWriter = new StringWriter();
            htmlContent.WriteTo(stringWriter, HtmlEncoder.Default);
            return stringWriter.ToString();
        }
    }
}