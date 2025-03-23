using System;
using System.IO;
using Extenso.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Extenso.AspNetCore.Mvc.ExtensoUI.Foundation;

public class FoundationTabsProvider : ITabsProvider
{
    private readonly string tabsId;

    public FoundationTabsProvider()
    {
        tabsId = $"tabs-{Guid.NewGuid()}";
    }

    #region ITabsProvider Members

    public void BeginTabs(Tabs tabs, TextWriter writer)
    {
        //var builder = new FluentTagBuilder("div", TagRenderMode.StartTag)
        //    .MergeAttributes(tabs.HtmlAttributes);

        //string tag = builder.ToString();

        //writer.Write(tag);
    }

    public void BeginTabsHeader(TextWriter writer) => writer.Write($@"<ul class=""tabs"" data-tabs id=""{tabsId}"">");

    public void BeginTabContent(TextWriter writer) => writer.Write($@"<div class=""tabs-content"" data-tabs-content=""{tabsId}"">");

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

    public void EndTabPanel(TextWriter writer) => writer.Write("</div>");

    public void EndTabsHeader(TextWriter writer) => writer.Write("</ul>");

    public void EndTabs(Tabs tabs, TextWriter writer) => writer.Write("</div>");

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