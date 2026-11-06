using Microsoft.AspNetCore.Mvc.Rendering;

namespace Extenso.AspNetCore.Mvc.ExtensoUI;

/// <summary>
/// Represents the builder base.
/// </summary>
/// <typeparam name="TModel">The type of the model.</typeparam>
/// <typeparam name="T">The type of the value.</typeparam>
public abstract class BuilderBase<TModel, T> : IDisposable where T : HtmlElement
{
    // Fields
    /// <summary>
    /// The element.
    /// </summary>
    protected readonly T Element;

    /// <summary>
    /// The text writer.
    /// </summary>
    protected readonly TextWriter TextWriter;
    /// <summary>
    /// The HTML helper.
    /// </summary>
    protected readonly IHtmlHelper<TModel> HtmlHelper;

    // Methods
    /// <summary>
    /// Initializes a new instance of the BuilderBase class.
    /// </summary>
    /// <param name="htmlHelper">The HTML helper.</param>
    /// <param name="element">The element.</param>
    public BuilderBase(IHtmlHelper<TModel> htmlHelper, T element)
    {
        Element = element ?? throw new ArgumentNullException(nameof(element));

        TextWriter = htmlHelper.ViewContext.Writer;
        Element.StartTag(TextWriter);
        HtmlHelper = htmlHelper;
    }

    /// <summary>
    /// Releases the resources used by this instance.
    /// </summary>
    public virtual void Dispose() => Element.EndTag(TextWriter);
}