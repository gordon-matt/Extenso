using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;

namespace Extenso.AspNetCore.Mvc.Html;

public static class HtmlContentExtensions
{
    public static string GetString(this IHtmlContent htmlContent)
    {
        using var stringWriter = new StringWriter();
        htmlContent.WriteTo(stringWriter, HtmlEncoder.Default);
        return stringWriter.ToString();
    }
}