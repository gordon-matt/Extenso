namespace Extenso.AspNetCore.Mvc.ExtensoUI;

/// <summary>
/// Defines the contract for the panel provider.
/// </summary>
public interface IPanelProvider
{
    /// <summary>
    /// Begins the panel.
    /// </summary>
    /// <param name="panel">The panel.</param>
    /// <param name="writer">The writer.</param>
    void BeginPanel(Panel panel, TextWriter writer);

    /// <summary>
    /// Begins the panel section.
    /// </summary>
    /// <param name="sectionType">The section type.</param>
    /// <param name="writer">The writer.</param>
    /// <param name="title">The title.</param>
    void BeginPanelSection(PanelSectionType sectionType, TextWriter writer, string title = null);

    /// <summary>
    /// Performs the end panel operation.
    /// </summary>
    /// <param name="panel">The panel.</param>
    /// <param name="writer">The writer.</param>
    void EndPanel(Panel panel, TextWriter writer);

    /// <summary>
    /// Performs the end panel section operation.
    /// </summary>
    /// <param name="sectionType">The section type.</param>
    /// <param name="writer">The writer.</param>
    void EndPanelSection(PanelSectionType sectionType, TextWriter writer);
}