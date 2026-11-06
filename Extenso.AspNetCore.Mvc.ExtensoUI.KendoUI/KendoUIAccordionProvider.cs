using Extenso.AspNetCore.Mvc.ExtensoUI.Providers;
using Extenso.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Extenso.AspNetCore.Mvc.ExtensoUI.KendoUI;

/// <summary>
/// Represents the kendo UI accordion provider.
/// </summary>
public class KendoUIAccordionProvider : IAccordionProvider
{
    private readonly BaseUIProvider uiProvider;

    /// <summary>
    /// Initializes a new instance of the KendoUIAccordionProvider class.
    /// </summary>
    /// <param name="uiProvider">The UI provider.</param>
    public KendoUIAccordionProvider(BaseUIProvider uiProvider)
    {
        this.uiProvider = uiProvider;
    }

    #region IAccordionProvider Members

    /// <summary>
    /// Begins the accordion.
    /// </summary>
    /// <param name="accordion">The accordion.</param>
    /// <param name="writer">The writer.</param>
    public void BeginAccordion(Accordion accordion, TextWriter writer)
    {
        uiProvider.Scripts.Add(
$@"$('#{accordion.Id}').kendoPanelBar({{
    expandMode: 'single'
}});");

        var builder = new FluentTagBuilder("ul", TagRenderMode.StartTag)
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
        var li = new FluentTagBuilder("li", TagRenderMode.StartTag);

        if (expanded)
        {
            li.AddCssClass("k-state-active");
        }

        writer.Write(li.ToString());
        writer.Write(title);
        writer.Write(@"<div class=""k-content"">");
    }

    /// <summary>
    /// Performs the end accordion operation.
    /// </summary>
    /// <param name="accordion">The accordion.</param>
    /// <param name="writer">The writer.</param>
    public void EndAccordion(Accordion accordion, TextWriter writer) => writer.Write("</ul>");

    /// <summary>
    /// Performs the end accordion panel operation.
    /// </summary>
    /// <param name="writer">The writer.</param>
    public void EndAccordionPanel(TextWriter writer) => writer.Write("</div></li>");

    #endregion IAccordionProvider Members
}