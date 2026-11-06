using Extenso.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Extenso.AspNetCore.Mvc.ExtensoUI.Foundation;

/// <summary>
/// Represents the foundation panel provider.
/// </summary>
public class FoundationPanelProvider : IPanelProvider
{
    #region IPanelProvider Members

    /// <summary>
    /// Begins the panel.
    /// </summary>
    /// <param name="panel">The panel.</param>
    /// <param name="writer">The writer.</param>
    public void BeginPanel(Panel panel, TextWriter writer)
    {
        switch (panel.State)
        {
            case State.Danger: panel.EnsureClass("callout alert"); break;
            case State.Info: panel.EnsureClass("callout secondary"); break;
            case State.Inverse: panel.EnsureClass("callout secondary"); break;
            case State.Primary: panel.EnsureClass("callout primary"); break;
            case State.Success: panel.EnsureClass("callout success"); break;
            case State.Warning: panel.EnsureClass("callout warning"); break;
            case State.Default:
            default: panel.EnsureClass("callout"); break;
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
        switch (sectionType)
        {
            case PanelSectionType.Heading:
                {
                    writer.Write($@"<div class=""panel-heading""><h5 class=""panel-title"">{title}</h5>");
                }
                break;

            case PanelSectionType.Body:
                writer.Write(@"<div class=""panel-body"">");
                break;

            case PanelSectionType.Footer:
                writer.Write(@"<div class=""panel-footer"">");
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