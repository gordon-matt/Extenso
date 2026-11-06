using Extenso.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Extenso.AspNetCore.Mvc.ExtensoUI;

/// <summary>
/// Represents the bootstrap5tabs provider.
/// </summary>
public class Bootstrap5TabsProvider : ITabsProvider
{
    #region ITabsProvider Members

    /// <summary>
    /// Begins the tabs.
    /// </summary>
    /// <param name="tabs">The tabs.</param>
    /// <param name="writer">The writer.</param>
    public void BeginTabs(Tabs tabs, TextWriter writer)
    {
        tabs.EnsureHtmlAttribute("role", "tabpanel");

        var builder = new TagBuilder("div")
        {
            TagRenderMode = TagRenderMode.StartTag
        };

        builder.MergeAttributes(tabs.HtmlAttributes);
        string tag = builder.Build();

        writer.Write(tag);
    }

    /// <summary>
    /// Begins the tabs header.
    /// </summary>
    /// <param name="writer">The writer.</param>
    public void BeginTabsHeader(TextWriter writer) => writer.Write(@"<ul class=""nav nav-tabs"" role=""tablist"">");

    /// <summary>
    /// Begins the tab content.
    /// </summary>
    /// <param name="writer">The writer.</param>
    public void BeginTabContent(TextWriter writer) => writer.Write(@"<div class=""tab-content"">");

    /// <summary>
    /// Begins the tab panel.
    /// </summary>
    /// <param name="panel">The panel.</param>
    /// <param name="writer">The writer.</param>
    public void BeginTabPanel(TabPanel panel, TextWriter writer)
    {
        var builder = new TagBuilder("div")
        {
            TagRenderMode = TagRenderMode.StartTag
        };

        builder.MergeAttribute("id", panel.Id);
        builder.MergeAttribute("role", "tabpanel");
        builder.AddCssClass("tab-pane");

        if (panel.IsActive)
        {
            builder.AddCssClass("active");
        }

        writer.Write(builder.Build());
    }

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
    public void EndTabs(Tabs tabs, TextWriter writer) => writer.Write("</div></div>");

    /// <summary>
    /// Writes the tab.
    /// </summary>
    /// <param name="writer">The writer.</param>
    /// <param name="label">The label.</param>
    /// <param name="tabId">The tab identifier.</param>
    /// <param name="isActive">The is active.</param>
    public void WriteTab(TextWriter writer, string label, string tabId, bool isActive) => writer.Write($@"<li class=""nav-item"" role=""presentation""><button type=""button"" class=""nav-link{(isActive ? " active" : string.Empty)}"" data-bs-toggle=""tab"" data-bs-target=""#{tabId}"" role=""tab"" aria-controls=""{tabId}"" aria-selected=""{(isActive ? "true" : "false")}"">{label}</button></li>");

    #endregion ITabsProvider Members
}