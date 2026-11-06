using Extenso.AspNetCore.Mvc.ExtensoUI.Providers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Extenso.AspNetCore.Mvc.ExtensoUI;

/// <summary>
/// Represents the HTML element.
/// </summary>
public abstract class HtmlElement
{
    /// <summary>
    /// Gets or sets the HTML attributes.
    /// </summary>
    public IDictionary<string, object> HtmlAttributes { get; private set; }

    /// <summary>
    /// Initializes a new instance of the HtmlElement class.
    /// </summary>
    /// <param name="htmlAttributes">The HTML attributes.</param>
    public HtmlElement(object htmlAttributes)
    {
        HtmlAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
    }

    internal IExtensoUIProvider Provider { get; set; }

    /// <summary>
    /// Starts the tag.
    /// </summary>
    /// <param name="textWriter">The text writer.</param>
    protected internal abstract void StartTag(TextWriter textWriter);

    /// <summary>
    /// Performs the end tag operation.
    /// </summary>
    /// <param name="textWriter">The text writer.</param>
    protected internal abstract void EndTag(TextWriter textWriter);

    /// <summary>
    /// Ensures the class.
    /// </summary>
    /// <param name="className">The class name.</param>
    public void EnsureClass(string className) => HtmlAttributes.EnsureCssClass(className);

    /// <summary>
    /// Ensures the HTML attribute.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    /// <param name="replaceExisting">The replace existing.</param>
    public void EnsureHtmlAttribute(string key, string value, bool replaceExisting = true) =>
        HtmlAttributes.EnsureHtmlAttribute(key, value, replaceExisting);
}