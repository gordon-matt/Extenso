using Extenso.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Extenso.AspNetCore.Mvc.ExtensoUI;

/// <summary>
/// Represents the bootstrap3accordion provider.
/// </summary>
public class Bootstrap3AccordionProvider : IAccordionProvider
{
    #region IAccordionProvider Members

    /// <summary>
    /// Begins the accordion.
    /// </summary>
    /// <param name="accordion">The accordion.</param>
    /// <param name="writer">The writer.</param>
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

    /// <summary>
    /// Begins the accordion panel.
    /// </summary>
    /// <param name="writer">The writer.</param>
    /// <param name="title">The title.</param>
    /// <param name="panelId">The panel identifier.</param>
    /// <param name="parentAccordionId">The parent accordion identifier.</param>
    /// <param name="expanded">The expanded.</param>
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

    /// <summary>
    /// Performs the end accordion operation.
    /// </summary>
    /// <param name="accordion">The accordion.</param>
    /// <param name="writer">The writer.</param>
    public void EndAccordion(Accordion accordion, TextWriter writer) => writer.Write("</div>");

    /// <summary>
    /// Performs the end accordion panel operation.
    /// </summary>
    /// <param name="writer">The writer.</param>
    public void EndAccordionPanel(TextWriter writer) => writer.Write("</div></div></div>");

    #endregion IAccordionProvider Members
}