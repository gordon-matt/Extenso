using Extenso.AspNetCore.Mvc.ExtensoUI.Providers;
using Extenso.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Extenso.AspNetCore.Mvc.ExtensoUI.KendoUI;

/// <summary>
/// Represents the kendo UI tabs provider.
/// </summary>
public class KendoUITabsProvider : ITabsProvider
{
    private readonly BaseUIProvider uiProvider;

    /// <summary>
    /// Initializes a new instance of the KendoUITabsProvider class.
    /// </summary>
    /// <param name="uiProvider">The UI provider.</param>
    public KendoUITabsProvider(BaseUIProvider uiProvider)
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
        uiProvider.Scripts.Add(
$@"$('#{tabs.Id}').kendoTabStrip({{
    animation:  {{
        open: {{
            effects: 'fadeIn'
        }}
    }}
}});");

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
    public void BeginTabPanel(TabPanel panel, TextWriter writer) => writer.Write("<div>");

    /// <summary>
    /// Performs the end tab panel operation.
    /// </summary>
    /// <param name="writer">The writer.</param>
    public void EndTabPanel(TextWriter writer) => writer.Write("</div>");

    /// <summary>
    /// Performs the end tabs header operation.
    /// </summary>
    /// <param name="writer">The writer.</param>
    public void EndTabsHeader(TextWriter writer) => writer.Write("</ul>");

    /// <summary>
    /// Performs the end tabs operation.
    /// </summary>
    /// <param name="tabs">The tabs.</param>
    /// <param name="writer">The writer.</param>
    public void EndTabs(Tabs tabs, TextWriter writer) => writer.Write("</div>");

    /// <summary>
    /// Writes the tab.
    /// </summary>
    /// <param name="writer">The writer.</param>
    /// <param name="label">The label.</param>
    /// <param name="tabId">The tab identifier.</param>
    /// <param name="isActive">The is active.</param>
    public void WriteTab(TextWriter writer, string label, string tabId, bool isActive)
    {
        if (isActive)
        {
            writer.Write($@"<li role=""tab"" aria-controls=""{tabId}"" class=""k-state-active"">{label}</li>");
        }
        else
        {
            writer.Write($@"<li role=""tab"" aria-controls=""{tabId}"">{label}</li>");
        }
    }

    #endregion ITabsProvider Members
}