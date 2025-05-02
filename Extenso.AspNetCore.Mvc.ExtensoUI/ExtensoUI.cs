using Extenso.AspNetCore.Mvc.ExtensoUI.Providers;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Extenso.AspNetCore.Mvc.ExtensoUI;

public class ExtensoUI<TModel>
{
    private readonly IHtmlHelper<TModel> html;
    private readonly IExtensoUIProvider provider;

    internal ExtensoUI(IHtmlHelper<TModel> html, IExtensoUIProvider provider = null)
    {
        this.html = html;
        this.provider = provider ?? ExtensoUISettings.DefaultAdminProvider;
    }

    public IHtmlContent RenderScripts() => provider.RenderScripts();

    #region Accordion

    public AccordionBuilder<TModel> Begin(Accordion accordion)
    {
        if (accordion == null)
        {
            throw new ArgumentNullException(nameof(accordion));
        }

        accordion.Provider = provider;
        return new AccordionBuilder<TModel>(html, accordion);
    }

    #endregion Accordion

    #region Buttons

    public IHtmlContent ModalLaunchButton(string modalId, string text, object htmlAttributes = null) => provider.ModalProvider.ModalLaunchButton(modalId, text, htmlAttributes);

    public IHtmlContent ActionLink(string text, State state, string actionName, string controllerName, object routeValues = null, object htmlAttributes = null) => provider.ActionLink(html, text, state, actionName, controllerName, routeValues, htmlAttributes);

    public IHtmlContent Button(string text, State state, string onClick = null, object htmlAttributes = null) => provider.Button(text, state, onClick, htmlAttributes);

    public IHtmlContent SubmitButton(string text, State state, object htmlAttributes = null) => provider.SubmitButton(text, state, htmlAttributes);

    #endregion Buttons

    #region Modal (Dialog)

    public ModalBuilder<TModel> Begin(Modal modal)
    {
        if (modal == null)
        {
            throw new ArgumentNullException(nameof(modal));
        }

        modal.Provider = provider;
        return new ModalBuilder<TModel>(html, modal);
    }

    #endregion Modal (Dialog)

    #region Panel

    public PanelBuilder<TModel> Begin(Panel panel)
    {
        if (panel == null)
        {
            throw new ArgumentNullException(nameof(panel));
        }

        panel.Provider = provider;
        return new PanelBuilder<TModel>(html, panel);
    }

    #endregion Panel

    #region Tabs

    public TabsBuilder<TModel> Begin(Tabs tabs)
    {
        if (tabs == null)
        {
            throw new ArgumentNullException(nameof(tabs));
        }

        tabs.Provider = provider;
        return new TabsBuilder<TModel>(html, tabs);
    }

    #endregion Tabs
}