using System.IO;
using Extenso.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Extenso.AspNetCore.Mvc.ExtensoUI;

public class Bootstrap3AccordionProvider : IAccordionProvider
{
    #region IAccordionProvider Members

    public void BeginAccordion(Accordion accordion, TextWriter writer)
    {
        accordion.EnsureClass("panel-group");
        accordion.EnsureHtmlAttribute("role", "tablist");
        accordion.EnsureHtmlAttribute("aria-multiselectable", "true");

        var builder = new FluentTagBuilder("div", TagRenderMode.StartTag)
            .MergeAttributes(accordion.HtmlAttributes);

        string tag = builder.ToString();

        writer.Write(tag);
    }

    public void BeginAccordionPanel(TextWriter writer, string title, string panelId, string parentAccordionId, bool expanded)
    {
        writer.Write(@"<div class=""panel panel-default"">");

        var panel = new FluentTagBuilder("div")
            .AddCssClass("panel-heading")
            .MergeAttribute("role", "tab")
            .MergeAttribute("id", $"{panelId}-heading")
            .StartTag("h4")
                .AddCssClass("panel-title")
                .StartTag("a")
                    .MergeAttribute("data-toggle", "collapse")
                    .MergeAttribute("data-parent", $"#{parentAccordionId}")
                    .MergeAttribute("href", $"#{panelId}")
                    .MergeAttribute("aria-controls", panelId)
                    .MergeAttribute("aria-expanded", expanded ? "true" : "false")
                    .SetInnerHtml(title)
                .EndTag()
            .EndTag();

        writer.Write(panel.ToString());

        var collapse = new FluentTagBuilder("div", TagRenderMode.StartTag)
            .AddCssClass(expanded ? "panel-collapse collapse in" : "panel-collapse collapse")
            .MergeAttribute("id", panelId)
            .MergeAttribute("role", "tabpanel")
            .MergeAttribute("aria-labelledby", $"{panelId}-heading");

        writer.Write(collapse.ToString());
        writer.Write(@"<div class=""panel-body"">");
    }

    public void EndAccordion(Accordion accordion, TextWriter writer) => writer.Write("</div>");

    public void EndAccordionPanel(TextWriter writer) => writer.Write("</div></div></div>");

    #endregion IAccordionProvider Members
}