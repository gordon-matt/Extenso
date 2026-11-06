using Extenso.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Extenso.AspNetCore.Mvc.ExtensoUI.Providers;

/// <summary>
/// Represents the base UI provider.
/// </summary>
public abstract class BaseUIProvider : IExtensoUIProvider
{
    /// <summary>
    /// Gets or sets the scripts.
    /// </summary>
    public ICollection<string> Scripts
    {
        get => field ??= [];
        set => field = value;
    }

    #region IExtensoUIProvider Members

    /// <summary>
    /// Renders the scripts.
    /// </summary>
    /// <returns>The scripts.</returns>
    public virtual IHtmlContent RenderScripts()
    {
        var result = new HtmlString(string.Join(Environment.NewLine, Scripts));
        Scripts.Clear();
        return result;
    }

    #region General

    /// <summary>
    /// Gets the action link.
    /// </summary>
    /// <param name="html">The HTML.</param>
    /// <param name="text">The text.</param>
    /// <param name="state">The state.</param>
    /// <param name="actionName">The action name.</param>
    /// <param name="controllerName">The controller name.</param>
    /// <param name="routeValues">The route values.</param>
    /// <param name="htmlAttributes">The HTML attributes.</param>
    /// <returns>The action link.</returns>
    public virtual IHtmlContent ActionLink(IHtmlHelper html, string text, State state, string actionName, string controllerName, object routeValues = null, object htmlAttributes = null)
    {
        string stateCss = GetButtonCssClass(state);

        var builder = new FluentTagBuilder("a")
            .MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes))
            .MergeAttribute("href", Internal.UrlHelper.Action(actionName, controllerName, routeValues))
            .AddCssClass(stateCss)
            .SetInnerHtml(text);

        return new HtmlString(builder.ToString());
    }

    /// <summary>
    /// Gets the button.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="state">The state.</param>
    /// <param name="onClick">The on click.</param>
    /// <param name="htmlAttributes">The HTML attributes.</param>
    /// <returns>The button.</returns>
    public virtual IHtmlContent Button(string text, State state, string onClick = null, object htmlAttributes = null)
    {
        string stateCss = GetButtonCssClass(state);

        var builder = new FluentTagBuilder("button")
            .MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes))
            .MergeAttribute("type", "button")
            .AddCssClass(stateCss)
            .SetInnerHtml(text);

        if (!string.IsNullOrEmpty(onClick))
        {
            builder.MergeAttribute("onclick", onClick);
        }

        return new HtmlString(builder.ToString());
    }

    /// <summary>
    /// Submits the button.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="state">The state.</param>
    /// <param name="htmlAttributes">The HTML attributes.</param>
    /// <returns>The button.</returns>
    public virtual IHtmlContent SubmitButton(string text, State state, object htmlAttributes = null)
    {
        string stateCss = GetButtonCssClass(state);

        var builder = new FluentTagBuilder("button")
            .MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes))
            .MergeAttribute("type", "submit")
            .AddCssClass(stateCss)
            .SetInnerHtml(text);

        return new HtmlString(builder.ToString());
    }

    #endregion General

    #region Special

    /// <summary>
    /// Gets the accordion provider.
    /// </summary>
    public abstract IAccordionProvider AccordionProvider { get; }

    /// <summary>
    /// Gets the modal provider.
    /// </summary>
    public abstract IModalProvider ModalProvider { get; }

    /// <summary>
    /// Gets the panel provider.
    /// </summary>
    public abstract IPanelProvider PanelProvider { get; }

    /// <summary>
    /// Gets the tabs provider.
    /// </summary>
    public abstract ITabsProvider TabsProvider { get; }

    #endregion Special

    #endregion IExtensoUIProvider Members

    /// <summary>
    /// Gets the button CSS class.
    /// </summary>
    /// <param name="state">The state.</param>
    /// <returns>The button CSS class.</returns>
    protected abstract string GetButtonCssClass(State state);
}