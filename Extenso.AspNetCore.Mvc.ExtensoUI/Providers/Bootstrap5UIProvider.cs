using Extenso.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Extenso.AspNetCore.Mvc.ExtensoUI.Providers;

public class Bootstrap5UIProvider : BaseUIProvider
{
    private IAccordionProvider accordionProvider;
    private IModalProvider modalProvider;
    private IPanelProvider panelProvider;
    private ITabsProvider tabsProvider;

    #region IExtensoUIProvider Members

    #region General

    public override IHtmlContent ActionLink(IHtmlHelper html, string text, State state, string actionName, string controllerName, object routeValues = null, object htmlAttributes = null)
    {
        string stateCss = GetButtonCssClass(state);

        var builder = new FluentTagBuilder("a")
            .MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes))
            .MergeAttribute("href", Internal.UrlHelper.Action(actionName, controllerName, routeValues))
            .AddCssClass(stateCss)
            .SetInnerHtml(text);

        return new HtmlString(builder.ToString());
    }

    public override IHtmlContent Button(string text, State state, string onClick = null, object htmlAttributes = null)
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

    public override IHtmlContent SubmitButton(string text, State state, object htmlAttributes = null)
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

    public override IAccordionProvider AccordionProvider => accordionProvider ??= new Bootstrap5AccordionProvider();

    public override IModalProvider ModalProvider => modalProvider ??= new Bootstrap5ModalProvider();

    public override IPanelProvider PanelProvider => panelProvider ??= new Bootstrap5PanelProvider();

    public override ITabsProvider TabsProvider => tabsProvider ??= new Bootstrap5TabsProvider();

    #endregion Special

    #endregion IExtensoUIProvider Members

    protected override string GetButtonCssClass(State state) => state switch
    {
        State.Danger => "btn btn-danger",
        State.Default => "btn btn-secondary",
        State.Info => "btn btn-info",
        State.Inverse => "btn btn-dark",
        State.Primary => "btn btn-primary",
        State.Success => "btn btn-success",
        State.Warning => "btn btn-warning",
        _ => "btn btn-secondary",
    };
}