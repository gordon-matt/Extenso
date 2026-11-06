using Microsoft.AspNetCore.Mvc.Rendering;

namespace Extenso.AspNetCore.Mvc.ExtensoUI;

/// <summary>
/// Represents the accordion builder.
/// </summary>
/// <typeparam name="TModel">The type of the model.</typeparam>
public class AccordionBuilder<TModel> : BuilderBase<TModel, Accordion>
{
    internal AccordionBuilder(IHtmlHelper<TModel> htmlHelper, Accordion accordion)
        : base(htmlHelper, accordion)
    {
    }

    /// <summary>
    /// Begins the panel.
    /// </summary>
    /// <param name="title">The title.</param>
    /// <param name="id">The identifier.</param>
    /// <param name="expanded">The expanded.</param>
    /// <returns>The panel.</returns>
    public AccordionPanel BeginPanel(string title, string id, bool expanded = false) => new(Element.Provider, TextWriter, title, id, Element.Id, expanded);

    /// <inheritdoc/>
    public override void Dispose() => base.Dispose();
}