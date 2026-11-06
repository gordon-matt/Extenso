using Microsoft.AspNetCore.Mvc.Rendering;

namespace Extenso.AspNetCore.Mvc.ExtensoUI;

/// <summary>
/// Represents the modal builder.
/// </summary>
/// <typeparam name="TModel">The type of the model.</typeparam>
public class ModalBuilder<TModel> : BuilderBase<TModel, Modal>
{
    internal ModalBuilder(IHtmlHelper<TModel> htmlHelper, Modal modal)
        : base(htmlHelper, modal)
    {
    }

    /// <summary>
    /// Begins the header.
    /// </summary>
    /// <param name="title">The title.</param>
    /// <returns>The header.</returns>
    public ModalSectionPanel BeginHeader(string title) => new(Element.Provider, ModalSection.Header, TextWriter, title);

    /// <summary>
    /// Begins the body.
    /// </summary>
    /// <returns>The body.</returns>
    public ModalSectionPanel BeginBody() => new(Element.Provider, ModalSection.Body, TextWriter);

    /// <summary>
    /// Begins the footer.
    /// </summary>
    /// <returns>The footer.</returns>
    public ModalSectionPanel BeginFooter() => new(Element.Provider, ModalSection.Footer, TextWriter);

    /// <inheritdoc/>
    public override void Dispose() => base.Dispose();
}