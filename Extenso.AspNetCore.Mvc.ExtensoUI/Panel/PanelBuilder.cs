using Microsoft.AspNetCore.Mvc.Rendering;

namespace Extenso.AspNetCore.Mvc.ExtensoUI;

/// <summary>
/// Represents the panel builder.
/// </summary>
/// <typeparam name="TModel">The type of the model.</typeparam>
public class PanelBuilder<TModel> : BuilderBase<TModel, Panel>
{
    internal PanelBuilder(IHtmlHelper<TModel> htmlHelper, Panel panel)
        : base(htmlHelper, panel)
    {
    }

    // TODO: Support html attributes
    /// <summary>
    /// Begins the header.
    /// </summary>
    /// <param name="title">The title.</param>
    /// <returns>The header.</returns>
    public PanelSection BeginHeader(string title) => new(Element.Provider, PanelSectionType.Heading, TextWriter, title);

    /// <summary>
    /// Begins the body.
    /// </summary>
    /// <returns>The body.</returns>
    public PanelSection BeginBody() => new(Element.Provider, PanelSectionType.Body, TextWriter);

    /// <summary>
    /// Begins the footer.
    /// </summary>
    /// <returns>The footer.</returns>
    public PanelSection BeginFooter() => new(Element.Provider, PanelSectionType.Footer, TextWriter);

    /// <inheritdoc/>
    public override void Dispose() => base.Dispose();
}