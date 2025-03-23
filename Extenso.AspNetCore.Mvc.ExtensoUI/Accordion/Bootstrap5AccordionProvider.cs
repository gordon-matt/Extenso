using System.IO;
using Extenso.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Extenso.AspNetCore.Mvc.ExtensoUI;

internal class Bootstrap5AccordionProvider : IAccordionProvider
{
    #region IAccordionProvider Members

    public void BeginAccordion(Accordion accordion, TextWriter writer)
    {
        accordion.EnsureClass("accordion");

        var builder = new FluentTagBuilder("div", TagRenderMode.StartTag)
            .MergeAttributes(accordion.HtmlAttributes);

        string tag = builder.ToString();

        writer.Write(tag);
    }

    public void BeginAccordionPanel(TextWriter writer, string title, string panelId, string parentAccordionId, bool expanded)
    {
        writer.Write(@"<div class=""accordion-item"">");

        var header = new FluentTagBuilder("h2")
            .AddCssClass("accordion-header")
            .MergeAttribute("id", $"{panelId}-heading")
            .StartTag("button")
                .AddCssClass($"accordion-button{(expanded ? string.Empty : " collapsed")}")
                .MergeAttribute("type", "button")
                .MergeAttribute("data-bs-toggle", "collapse")
                .MergeAttribute("data-bs-target", $"#{panelId}")
                .MergeAttribute("aria-controls", panelId)
                .MergeAttribute("aria-expanded", expanded ? "true" : "false")
                .SetInnerHtml(title)
            .EndTag();

        writer.Write(header.ToString());

        var collapse = new FluentTagBuilder("div", TagRenderMode.StartTag)
            .AddCssClass($"accordion-collapse collapse{(expanded ? " show" : string.Empty)}")
            .MergeAttribute("id", panelId)
            .MergeAttribute("aria-labelledby", $"{panelId}-heading")
            .MergeAttribute("data-bs-parent", $"#{parentAccordionId}");

        writer.Write(collapse.ToString());
        writer.Write(@"<div class=""accordion-body"">");
    }

    public void EndAccordion(Accordion accordion, TextWriter writer) => writer.Write("</div>");

    public void EndAccordionPanel(TextWriter writer) => writer.Write("</div></div></div>");

    #endregion IAccordionProvider Members
}