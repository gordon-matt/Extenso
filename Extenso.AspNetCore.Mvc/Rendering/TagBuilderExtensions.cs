using System.IO;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Extenso.AspNetCore.Mvc.Rendering;

public static class TagBuilderExtensions
{
    public static string Build(this TagBuilder tagBuilder)
    {
        using var stringWriter = new StringWriter();
        tagBuilder.WriteTo(stringWriter, HtmlEncoder.Default);
        return stringWriter.ToString();
    }
}