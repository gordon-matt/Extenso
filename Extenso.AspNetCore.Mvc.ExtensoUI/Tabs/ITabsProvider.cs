using System.IO;

namespace Extenso.AspNetCore.Mvc.ExtensoUI;

public interface ITabsProvider
{
    void BeginTabs(Tabs tabs, TextWriter writer);

    void BeginTabsHeader(TextWriter writer);

    void BeginTabContent(TextWriter writer);

    void BeginTabPanel(TabPanel panel, TextWriter writer);

    void EndTabPanel(TextWriter writer);

    void EndTabsHeader(TextWriter writer);

    void EndTabs(Tabs tabs, TextWriter writer);

    void WriteTab(TextWriter writer, string label, string tabId, bool isActive);
}