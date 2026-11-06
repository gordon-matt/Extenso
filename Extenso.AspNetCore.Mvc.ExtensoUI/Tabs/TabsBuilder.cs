using Microsoft.AspNetCore.Mvc.Rendering;

namespace Extenso.AspNetCore.Mvc.ExtensoUI;

/// <summary>
/// Represents the tabs builder.
/// </summary>
/// <typeparam name="TModel">The type of the model.</typeparam>
public class TabsBuilder<TModel> : BuilderBase<TModel, Tabs>
{
    private bool isHeaderClosed;
    private readonly Queue<string> tabIds;
    private bool writingContent;
    private string activeTabId;

    private bool isFirstTab = true;

    internal TabsBuilder(IHtmlHelper<TModel> htmlHelper, Tabs tabs)
        : base(htmlHelper, tabs)
    {
        tabIds = new Queue<string>();
        isHeaderClosed = false;
        writingContent = false;
        Element.Provider.TabsProvider.BeginTabsHeader(TextWriter);
    }

    /// <summary>
    /// Begins the panel.
    /// </summary>
    /// <returns>The panel.</returns>
    public TabPanel BeginPanel()
    {
        writingContent = true;
        CloseHeader();
        if (tabIds.Count == 0)
        {
            throw new InvalidOperationException("Tab definition not found. Use AddTab() before creating a new panel.");
        }

        string tabId = tabIds.Dequeue();
        if (tabId == activeTabId)
        {
            Element.Provider.TabsProvider.BeginTabContent(TextWriter);
            isFirstTab = false;
            return new TabPanel(Element.Provider, TextWriter, tabId, true);
        }

        return new TabPanel(Element.Provider, TextWriter, tabId);
    }

    private void CheckBuilderState()
    {
        if (writingContent)
        {
            throw new InvalidOperationException("Tab definition cannot be mixed with content panels.");
        }
    }

    private void CloseHeader()
    {
        if (!isHeaderClosed)
        {
            Element.Provider.TabsProvider.EndTabsHeader(TextWriter);
            isHeaderClosed = true;
        }
    }

    /// <inheritdoc/>
    public override void Dispose()
    {
        CloseHeader();

        // Close Tab Content Div:
        //element.Provider.TabsProvider.EndTabs((element as Tabs), textWriter);
        base.Dispose();
    }

    /// <summary>
    /// Performs the tab operation.
    /// </summary>
    /// <param name="label">The label.</param>
    /// <param name="id">The identifier.</param>
    public void Tab(string label, string id)
    {
        if (string.IsNullOrWhiteSpace(label))
        {
            throw new ArgumentNullException(nameof(label));
        }
        if (string.IsNullOrWhiteSpace(id))
        {
            throw new ArgumentNullException(nameof(id));
        }

        CheckBuilderState();
        string tabId = id;
        tabIds.Enqueue(tabId);

        if (isFirstTab)
        {
            activeTabId = tabId;
            Element.Provider.TabsProvider.WriteTab(TextWriter, label, tabId, true);
            isFirstTab = false;
        }
        else
        {
            Element.Provider.TabsProvider.WriteTab(TextWriter, label, tabId, false);
        }
    }
}