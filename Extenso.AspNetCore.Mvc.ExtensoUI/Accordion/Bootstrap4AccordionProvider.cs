using Extenso.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Extenso.AspNetCore.Mvc.ExtensoUI;

/// <summary>
/// Represents the bootstrap4accordion provider.
/// </summary>
public class Bootstrap4AccordionProvider : IAccordionProvider
{
    #region IAccordionProvider Members

    /// <summary>
    /// Begins the accordion.
    /// </summary>
    /// <param name="accordion">The accordion.</param>
    /// <param name="writer">The writer.</param>
    public void BeginAccordion(Accordion accordion, TextWriter writer)
    {
        accordion.EnsureClass("accordion");

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
        writer.Write(@"<div class=""card"">");

        var panel = new FluentTagBuilder("div")
            .AddCssClass("card-header")
            .MergeAttribute("id", $"{panelId}-heading")
            .StartTag("h5")
                .AddCssClass("mb-0")
                .StartTag("button")
                    .AddCssClass($"btn btn-link{(expanded ? string.Empty : " collapsed")}")
                    .MergeAttribute("type", "button")
                    .MergeAttribute("data-toggle", "collapse")
                    .MergeAttribute("data-target", $"#{panelId}")
                    .MergeAttribute("aria-controls", panelId)
                    .MergeAttribute("aria-expanded", expanded ? "true" : "false")
                    .SetInnerHtml(title)
                .EndTag()
            .EndTag();

        writer.Write(panel.ToString());

        var collapse = new FluentTagBuilder("div", TagRenderMode.StartTag)
            .AddCssClass($"collapse{(expanded ? " show" : string.Empty)}")
            .MergeAttribute("id", panelId)
            .MergeAttribute("aria-labelledby", $"{panelId}-heading")
            .MergeAttribute("data-parent", $"#{parentAccordionId}");

        writer.Write(collapse.ToString());
        writer.Write(@"<div class=""card-body"">");
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