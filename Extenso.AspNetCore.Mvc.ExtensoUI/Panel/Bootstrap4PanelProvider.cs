using Extenso.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Extenso.AspNetCore.Mvc.ExtensoUI;

/// <summary>
/// Represents the bootstrap4panel provider.
/// </summary>
public class Bootstrap4PanelProvider : IPanelProvider
{
    private Panel panel;

    #region IPanelProvider Members

    /// <summary>
    /// Begins the panel.
    /// </summary>
    /// <param name="panel">The panel.</param>
    /// <param name="writer">The writer.</param>
    public void BeginPanel(Panel panel, TextWriter writer)
    {
        this.panel = panel;

        switch (panel.State)
        {
            case State.Default: panel.EnsureClass("card"); break;
            default: panel.EnsureClass("card"); break;
        }

        var builder = new FluentTagBuilder("div", TagRenderMode.StartTag)
            .MergeAttributes(panel.HtmlAttributes);

        string tag = builder.ToString();

        writer.Write(tag);
    }

    /// <summary>
    /// Begins the panel section.
    /// </summary>
    /// <param name="sectionType">The section type.</param>
    /// <param name="writer">The writer.</param>
    /// <param name="title">The title.</param>
    public void BeginPanelSection(PanelSectionType sectionType, TextWriter writer, string title = null)
    {
        string headerClass = panel.State switch
        {
            State.Default => "bg-light",
            State.Danger => "bg-danger",
            State.Info => "bg-info",
            State.Inverse => "bg-dark",
            State.Success => "bg-success",
            State.Warning => "bg-warning",
            _ => "bg-primary",
        };

        switch (sectionType)
        {
            case PanelSectionType.Heading:
                writer.Write($@"<div class=""card-header {headerClass} text-white"">{title}");
                break;

            case PanelSectionType.Body:
                writer.Write(@"<div class=""card-body"">");
                break;

            case PanelSectionType.Footer:
                writer.Write(@"<div class=""card-footer"">");
                break;
        }
    }

    /// <summary>
    /// Performs the end panel operation.
    /// </summary>
    /// <param name="panel">The panel.</param>
    /// <param name="writer">The writer.</param>
    public void EndPanel(Panel panel, TextWriter writer) => writer.Write("</div>");

    /// <summary>
    /// Performs the end panel section operation.
    /// </summary>
    /// <param name="sectionType">The section type.</param>
    /// <param name="writer">The writer.</param>
    public void EndPanelSection(PanelSectionType sectionType, TextWriter writer) => writer.Write("</div>");

    #endregion IPanelProvider Members
}