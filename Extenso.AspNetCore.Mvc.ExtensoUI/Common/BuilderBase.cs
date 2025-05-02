using Microsoft.AspNetCore.Mvc.Rendering;

namespace Extenso.AspNetCore.Mvc.ExtensoUI;

public abstract class BuilderBase<TModel, T> : IDisposable where T : HtmlElement
{
    // Fields
    protected readonly T Element;

    protected readonly TextWriter TextWriter;
    protected readonly IHtmlHelper<TModel> HtmlHelper;

    // Methods
    public BuilderBase(IHtmlHelper<TModel> htmlHelper, T element)
    {
        Element = element ?? throw new ArgumentNullException(nameof(element));

        TextWriter = htmlHelper.ViewContext.Writer;
        Element.StartTag(TextWriter);
        HtmlHelper = htmlHelper;
    }

    public virtual void Dispose() => Element.EndTag(TextWriter);
}