using Extenso.AspNetCore.Mvc.ExtensoUI.Providers;
using Extenso.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Extenso.AspNetCore.Mvc.ExtensoUI.JQueryUI;

/// <summary>
/// Represents the j query UI accordion provider.
/// </summary>
public class JQueryUIAccordionProvider : IAccordionProvider
{
    private readonly BaseUIProvider uiProvider;

    /// <summary>
    /// Initializes a new instance of the JQueryUIAccordionProvider class.
    /// </summary>
    /// <param name="uiProvider">The UI provider.</param>
    public JQueryUIAccordionProvider(BaseUIProvider uiProvider)
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
        uiProvider.Scripts.Add($@"$('#{accordion.Id}').accordion();");

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
    public void BeginAccordionPanel(TextWriter writer, string title, string panelId, string parentAccordionId, bool expanded) => writer.Write($"<h3>{title}</h3><div>");

    /// <summary>
    /// Performs the end accordion panel operation.
    /// </summary>
    /// <param name="writer">The writer.</param>
    public void EndAccordionPanel(TextWriter writer) => writer.Write("</div>");

    /// <summary>
    /// Performs the end accordion operation.
    /// </summary>
    /// <param name="accordion">The accordion.</param>
    /// <param name="writer">The writer.</param>
    public void EndAccordion(Accordion accordion, TextWriter writer) => writer.Write("</div>");

    #endregion IAccordionProvider Members
}