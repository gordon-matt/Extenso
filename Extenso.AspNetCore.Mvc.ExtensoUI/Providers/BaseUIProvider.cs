using Extenso.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Extenso.AspNetCore.Mvc.ExtensoUI.Providers;

public abstract class BaseUIProvider : IExtensoUIProvider
{
    public ICollection<string> Scripts
    {
        get => field ??= [];
        set => field = value;
    }

    #region IExtensoUIProvider Members

    public virtual IHtmlContent RenderScripts()
    {
        var result = new HtmlString(string.Join(Environment.NewLine, Scripts));
        Scripts.Clear();
        return result;
    }

    #region General

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

    public abstract IAccordionProvider AccordionProvider { get; }

    public abstract IModalProvider ModalProvider { get; }

    public abstract IPanelProvider PanelProvider { get; }

    public abstract ITabsProvider TabsProvider { get; }

    #endregion Special

    #endregion IExtensoUIProvider Members

    protected abstract string GetButtonCssClass(State state);
}