using System.IO;
using Extenso.AspNetCore.Mvc.ExtensoUI.Providers;
using Extenso.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Extenso.AspNetCore.Mvc.ExtensoUI.KendoUI
{
    public class KendoUITabsProvider : ITabsProvider
    {
        private readonly BaseUIProvider uiProvider;

        public KendoUITabsProvider(BaseUIProvider uiProvider)
        {
            this.uiProvider = uiProvider;
        }

        #region ITabsProvider Members

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

        public void BeginTabsHeader(TextWriter writer)
        {
            writer.Write("<ul>");
        }

        public void BeginTabContent(TextWriter writer)
        {
        }

        public void BeginTabPanel(TabPanel panel, TextWriter writer)
        {
            writer.Write("<div>");
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
            writer.Write("</div>");
        }

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
}