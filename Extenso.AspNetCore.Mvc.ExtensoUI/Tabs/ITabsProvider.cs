namespace Extenso.AspNetCore.Mvc.ExtensoUI;

/// <summary>
/// Defines the contract for the tabs provider.
/// </summary>
public interface ITabsProvider
{
    /// <summary>
    /// Begins the tabs.
    /// </summary>
    /// <param name="tabs">The tabs.</param>
    /// <param name="writer">The writer.</param>
    void BeginTabs(Tabs tabs, TextWriter writer);

    /// <summary>
    /// Begins the tabs header.
    /// </summary>
    /// <param name="writer">The writer.</param>
    void BeginTabsHeader(TextWriter writer);

    /// <summary>
    /// Begins the tab content.
    /// </summary>
    /// <param name="writer">The writer.</param>
    void BeginTabContent(TextWriter writer);

    /// <summary>
    /// Begins the tab panel.
    /// </summary>
    /// <param name="panel">The panel.</param>
    /// <param name="writer">The writer.</param>
    void BeginTabPanel(TabPanel panel, TextWriter writer);

    /// <summary>
    /// Performs the end tab panel operation.
    /// </summary>
    /// <param name="writer">The writer.</param>
    void EndTabPanel(TextWriter writer);

    /// <summary>
    /// Performs the end tabs header operation.
    /// </summary>
    /// <param name="writer">The writer.</param>
    void EndTabsHeader(TextWriter writer);

    /// <summary>
    /// Performs the end tabs operation.
    /// </summary>
    /// <param name="tabs">The tabs.</param>
    /// <param name="writer">The writer.</param>
    void EndTabs(Tabs tabs, TextWriter writer);

    /// <summary>
    /// Writes the tab.
    /// </summary>
    /// <param name="writer">The writer.</param>
    /// <param name="label">The label.</param>
    /// <param name="tabId">The tab identifier.</param>
    /// <param name="isActive">The is active.</param>
    void WriteTab(TextWriter writer, string label, string tabId, bool isActive);
}