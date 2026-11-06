using Extenso.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Extenso.AspNetCore.Mvc.ExtensoUI.Foundation;

/// <summary>
/// Represents the foundation tabs provider.
/// </summary>
public class FoundationTabsProvider : ITabsProvider
{
    private readonly string tabsId;

    /// <summary>
    /// Initializes a new instance of the FoundationTabsProvider class.
    /// </summary>
    public FoundationTabsProvider()
    {
        tabsId = $"tabs-{Guid.NewGuid()}";
    }

    #region ITabsProvider Members

    /// <summary>
    /// Begins the tabs.
    /// </summary>
    /// <param name="tabs">The tabs.</param>
    /// <param name="writer">The writer.</param>
    public void BeginTabs(Tabs tabs, TextWriter writer)
    {
        //var builder = new FluentTagBuilder("div", TagRenderMode.StartTag)
        //    .MergeAttributes(tabs.HtmlAttributes);

        //string tag = builder.ToString();

        //writer.Write(tag);
    }

    /// <summary>
    /// Begins the tabs header.
    /// </summary>
    /// <param name="writer">The writer.</param>
    public void BeginTabsHeader(TextWriter writer) => writer.Write($@"<ul class=""tabs"" data-tabs id=""{tabsId}"">");

    /// <summary>
    /// Begins the tab content.
    /// </summary>
    /// <param name="writer">The writer.</param>
    public void BeginTabContent(TextWriter writer) => writer.Write($@"<div class=""tabs-content"" data-tabs-content=""{tabsId}"">");

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
        builder.AddCssClass("tabs-panel");

        if (panel.IsActive)
        {
            builder.AddCssClass("is-active");
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
            writer.Write($@"<li class=""tabs-title is-active""><a data-tabs-target=""{tabId}"" href=""#{tabId}"" aria-selected=""true"">{label}</a></li>");
        }
        else
        {
            writer.Write($@"<li class=""tabs-title""><a data-tabs-target=""{tabId}"" href=""#{tabId}"">{label}</a></li>");
        }
    }

    #endregion ITabsProvider Members
}