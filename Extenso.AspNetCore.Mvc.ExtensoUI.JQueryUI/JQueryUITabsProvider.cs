using System.IO;
using Extenso.AspNetCore.Mvc.ExtensoUI.Providers;
using Extenso.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Extenso.AspNetCore.Mvc.ExtensoUI.JQueryUI;

public class JQueryUITabsProvider : ITabsProvider
{
    private readonly BaseUIProvider uiProvider;

    public JQueryUITabsProvider(BaseUIProvider uiProvider)
    {
        this.uiProvider = uiProvider;
    }

    #region ITabsProvider Members

    public void BeginTabs(Tabs tabs, TextWriter writer)
    {
        uiProvider.Scripts.Add($@"$('#{tabs.Id}').tabs();");

        var builder = new FluentTagBuilder("div", TagRenderMode.StartTag)
            .MergeAttributes(tabs.HtmlAttributes);

        string tag = builder.ToString();

        writer.Write(tag);
    }

    public void BeginTabsHeader(TextWriter writer) => writer.Write("<ul>");

    public void WriteTab(TextWriter writer, string label, string tabId, bool isActive) => writer.Write($@"<li><a href=""#{tabId}"">{label}</a></li>");

    public void EndTabsHeader(TextWriter writer) => writer.Write("</ul>");

    public void BeginTabContent(TextWriter writer)
    {
    }

    public void BeginTabPanel(TabPanel panel, TextWriter writer) => writer.Write($@"<div id=""{panel.Id}"">");

    public void EndTabPanel(TextWriter writer) => writer.Write("</div>");

    public void EndTabs(Tabs tabs, TextWriter writer) => writer.Write("</div>");

    #endregion ITabsProvider Members
}