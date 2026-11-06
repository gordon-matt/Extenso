using Extenso.AspNetCore.Mvc.ExtensoUI.Providers;

namespace Extenso.AspNetCore.Mvc.ExtensoUI;

/// <summary>
/// Represents the tab panel.
/// </summary>
public class TabPanel : IDisposable
{
    private readonly TextWriter textWriter;
    private readonly IExtensoUIProvider provider;

    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    public string Id { get; private set; }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is active.
    /// </summary>
    public bool IsActive { get; private set; }

    internal TabPanel(IExtensoUIProvider provider, TextWriter writer, string id, bool isActive = false)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            throw new ArgumentNullException(nameof(id));
        }

        this.provider = provider;
        Id = id;
        IsActive = isActive;
        textWriter = writer;
        provider.TabsProvider.BeginTabPanel(this, textWriter);
    }

    /// <summary>
    /// Releases the resources used by this instance.
    /// </summary>
    public void Dispose() => provider.TabsProvider.EndTabPanel(textWriter);
}