using System.IO;
using Extenso.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Extenso.AspNetCore.Mvc.ExtensoUI;

public class Bootstrap5TabsProvider : ITabsProvider
{
    #region ITabsProvider Members

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

    public void BeginTabsHeader(TextWriter writer)
    {
        writer.Write(@"<ul class=""nav nav-tabs"" role=""tablist"">");
    }

    public void BeginTabContent(TextWriter writer)
    {
        writer.Write(@"<div class=""tab-content"">");
    }

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

    public void EndTabPanel(TextWriter writer)
    {
        writer.Write("</div>");
    }

    public void EndTabsHeader(TextWriter writer)
    {
        writer.Write("</ul>");
    }

    public void EndTabs(Tabs tabs, TextWriter writer)
    {
        writer.Write("</div></div>");
    }

    public void WriteTab(TextWriter writer, string label, string tabId, bool isActive)
    {
        writer.Write($@"<li role=""presentation"" class=""nav-item""><button class=""nav-link{(isActive ? " active" : string.Empty)}"" data-bs-target=""#{tabId}"" aria-controls=""{tabId}"" role=""tab"" data-bs-toggle=""tab"">{label}</button></li>");
    }

    #endregion ITabsProvider Members
}
