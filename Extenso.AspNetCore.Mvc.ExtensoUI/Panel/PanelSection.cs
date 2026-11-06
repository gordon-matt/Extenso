using Extenso.AspNetCore.Mvc.ExtensoUI.Providers;

namespace Extenso.AspNetCore.Mvc.ExtensoUI;

/// <summary>
/// Specifies the panel section type.
/// </summary>
public enum PanelSectionType : byte
{
    /// <summary>
    /// The heading value.
    /// </summary>
    Heading,
    /// <summary>
    /// The body value.
    /// </summary>
    Body,
    /// <summary>
    /// The footer value.
    /// </summary>
    Footer
}

/// <summary>
/// Represents the panel section.
/// </summary>
public class PanelSection : IDisposable
{
    private readonly TextWriter textWriter;
    private readonly IExtensoUIProvider provider;

    /// <summary>
    /// Gets or sets the section type.
    /// </summary>
    public PanelSectionType SectionType { get; private set; }

    // TODO: Support html attributes
    internal PanelSection(IExtensoUIProvider provider, PanelSectionType sectionType, TextWriter writer, string title = null)
    {
        this.provider = provider;
        SectionType = sectionType;
        textWriter = writer;
        provider.PanelProvider.BeginPanelSection(SectionType, textWriter, title);
    }

    /// <summary>
    /// Releases the resources used by this instance.
    /// </summary>
    public void Dispose() => provider.PanelProvider.EndPanelSection(SectionType, textWriter);
}