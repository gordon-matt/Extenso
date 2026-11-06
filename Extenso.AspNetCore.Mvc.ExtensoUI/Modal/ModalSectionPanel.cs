using Extenso.AspNetCore.Mvc.ExtensoUI.Providers;
using Microsoft.AspNetCore.Html;

namespace Extenso.AspNetCore.Mvc.ExtensoUI;

/// <summary>
/// Specifies the modal section.
/// </summary>
public enum ModalSection
{
    /// <summary>
    /// The header value.
    /// </summary>
    Header,
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
/// Represents the modal section panel.
/// </summary>
public class ModalSectionPanel : IDisposable
{
    private readonly TextWriter textWriter;
    private readonly IExtensoUIProvider provider;

    /// <summary>
    /// Gets or sets the section.
    /// </summary>
    public ModalSection Section { get; private set; }

    internal ModalSectionPanel(IExtensoUIProvider provider, ModalSection section, TextWriter writer, string title = null)
    {
        this.provider = provider;
        Section = section;
        textWriter = writer;
        provider.ModalProvider.BeginModalSectionPanel(Section, textWriter, title);
    }

    /// <summary>
    /// Gets the modal close button.
    /// </summary>
    /// <param name="modalId">The modal identifier.</param>
    /// <param name="text">The text.</param>
    /// <param name="htmlAttributes">The HTML attributes.</param>
    /// <returns>The modal close button.</returns>
    public IHtmlContent ModalCloseButton(string modalId, string text, object htmlAttributes = null) => provider.ModalProvider.ModalCloseButton(modalId, text, htmlAttributes);

    /// <summary>
    /// Releases the resources used by this instance.
    /// </summary>
    public void Dispose() => provider.ModalProvider.EndModalSectionPanel(Section, textWriter);
}