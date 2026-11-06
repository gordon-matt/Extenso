namespace Extenso.AspNetCore.Mvc.ExtensoUI;

/// <summary>
/// Defines the contract for the accordion provider.
/// </summary>
public interface IAccordionProvider
{
    /// <summary>
    /// Begins the accordion.
    /// </summary>
    /// <param name="accordion">The accordion.</param>
    /// <param name="writer">The writer.</param>
    void BeginAccordion(Accordion accordion, TextWriter writer);

    /// <summary>
    /// Begins the accordion panel.
    /// </summary>
    /// <param name="writer">The writer.</param>
    /// <param name="title">The title.</param>
    /// <param name="panelId">The panel identifier.</param>
    /// <param name="parentAccordionId">The parent accordion identifier.</param>
    /// <param name="expanded">The expanded.</param>
    void BeginAccordionPanel(TextWriter writer, string title, string panelId, string parentAccordionId, bool expanded);

    /// <summary>
    /// Performs the end accordion operation.
    /// </summary>
    /// <param name="accordion">The accordion.</param>
    /// <param name="writer">The writer.</param>
    void EndAccordion(Accordion accordion, TextWriter writer);

    /// <summary>
    /// Performs the end accordion panel operation.
    /// </summary>
    /// <param name="writer">The writer.</param>
    void EndAccordionPanel(TextWriter writer);
}