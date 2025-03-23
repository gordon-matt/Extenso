using System.IO;
using Extenso.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Extenso.AspNetCore.Mvc.ExtensoUI.Foundation;

public class FoundationAccordionProvider : IAccordionProvider
{
    #region IAccordionProvider Members

    public void BeginAccordion(Accordion accordion, TextWriter writer)
    {
        accordion.EnsureClass("accordion");
        accordion.EnsureHtmlAttribute("data-accordion", string.Empty);

        var builder = new FluentTagBuilder("ul", TagRenderMode.StartTag)
            .MergeAttributes(accordion.HtmlAttributes);

        string tag = builder.ToString();

        writer.Write(tag);
    }

    public void BeginAccordionPanel(TextWriter writer, string title, string panelId, string parentAccordionId, bool expanded)
    {
        var li = new FluentTagBuilder("li", TagRenderMode.StartTag)
            .AddCssClass("accordion-item");

        if (expanded)
        {
            li.AddCssClass("is-active");
        }

        writer.Write(li.ToString());
        writer.Write($@"<a href=""#"" class=""accordion-title"">{title}</a>");
        writer.Write(@"<div class=""accordion-content"" data-tab-content>");
    }

    public void EndAccordionPanel(TextWriter writer) => writer.Write("</div></li>");

    public void EndAccordion(Accordion accordion, TextWriter writer) => writer.Write("</ul>");

    #endregion IAccordionProvider Members
}