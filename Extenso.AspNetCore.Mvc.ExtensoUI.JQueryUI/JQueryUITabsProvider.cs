using Extenso.AspNetCore.Mvc.ExtensoUI.Providers;
using Extenso.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Extenso.AspNetCore.Mvc.ExtensoUI.JQueryUI;

/// <summary>
/// Represents the j query UI tabs provider.
/// </summary>
public class JQueryUITabsProvider : ITabsProvider
{
    private readonly BaseUIProvider uiProvider;

    /// <summary>
    /// Initializes a new instance of the JQueryUITabsProvider class.
    /// </summary>
    /// <param name="uiProvider">The UI provider.</param>
    public JQueryUITabsProvider(BaseUIProvider uiProvider)
    {
        this.uiProvider = uiProvider;
    }

    #region ITabsProvider Members

    /// <summary>
    /// Begins the tabs.
    /// </summary>
    /// <param name="tabs">The tabs.</param>
    /// <param name="writer">The writer.</param>
    public void BeginTabs(Tabs tabs, TextWriter writer)
    {
        uiProvider.Scripts.Add($@"$('#{tabs.Id}').tabs();");

        var builder = new FluentTagBuilder("div", TagRenderMode.StartTag)
            .MergeAttributes(tabs.HtmlAttributes);

        string tag = builder.ToString();

        writer.Write(tag);
    }

    /// <summary>
    /// Begins the tabs header.
    /// </summary>
    /// <param name="writer">The writer.</param>
    public void BeginTabsHeader(TextWriter writer) => writer.Write("<ul>");

    /// <summary>
    /// Writes the tab.
    /// </summary>
    /// <param name="writer">The writer.</param>
    /// <param name="label">The label.</param>
    /// <param name="tabId">The tab identifier.</param>
    /// <param name="isActive">The is active.</param>
    public void WriteTab(TextWriter writer, string label, string tabId, bool isActive) => writer.Write($@"<li><a href=""#{tabId}"">{label}</a></li>");

    /// <summary>
    /// Performs the end tabs header operation.
    /// </summary>
    /// <param name="writer">The writer.</param>
    public void EndTabsHeader(TextWriter writer) => writer.Write("</ul>");

    /// <summary>
    /// Begins the tab content.
    /// </summary>
    /// <param name="writer">The writer.</param>
    public void BeginTabContent(TextWriter writer)
    {
    }

    /// <summary>
    /// Begins the tab panel.
    /// </summary>
    /// <param name="panel">The panel.</param>
    /// <param name="writer">The writer.</param>
    public void BeginTabPanel(TabPanel panel, TextWriter writer) => writer.Write($@"<div id=""{panel.Id}"">");

    /// <summary>
    /// Performs the end tab panel operation.
    /// </summary>
    /// <param name="writer">The writer.</param>
    public void EndTabPanel(TextWriter writer) => writer.Write("</div>");

    /// <summary>
    /// Performs the end tabs operation.
    /// </summary>
    /// <param name="tabs">The tabs.</param>
    /// <param name="writer">The writer.</param>
    public void EndTabs(Tabs tabs, TextWriter writer) => writer.Write("</div>");

    #endregion ITabsProvider Members
}