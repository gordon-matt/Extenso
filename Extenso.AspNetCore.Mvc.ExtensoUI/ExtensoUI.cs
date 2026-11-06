using Extenso.AspNetCore.Mvc.ExtensoUI.Providers;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Extenso.AspNetCore.Mvc.ExtensoUI;

/// <summary>
/// Represents the extenso UI.
/// </summary>
/// <typeparam name="TModel">The type of the model.</typeparam>
public class ExtensoUI<TModel>
{
    private readonly IHtmlHelper<TModel> html;
    private readonly IExtensoUIProvider provider;

    internal ExtensoUI(IHtmlHelper<TModel> html, IExtensoUIProvider provider = null)
    {
        this.html = html;
        this.provider = provider ?? ExtensoUISettings.DefaultAdminProvider;
    }

    /// <summary>
    /// Renders the scripts.
    /// </summary>
    /// <returns>The scripts.</returns>
    public IHtmlContent RenderScripts() => provider.RenderScripts();

    #region Accordion

    /// <summary>
    /// Begins the value.
    /// </summary>
    /// <param name="accordion">The accordion.</param>
    /// <returns>The t accordion builder.</returns>
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

    /// <summary>
    /// Gets the modal launch button.
    /// </summary>
    /// <param name="modalId">The modal identifier.</param>
    /// <param name="text">The text.</param>
    /// <param name="htmlAttributes">The HTML attributes.</param>
    /// <returns>The modal launch button.</returns>
    public IHtmlContent ModalLaunchButton(string modalId, string text, object htmlAttributes = null) => provider.ModalProvider.ModalLaunchButton(modalId, text, htmlAttributes);

    /// <summary>
    /// Gets the action link.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="state">The state.</param>
    /// <param name="actionName">The action name.</param>
    /// <param name="controllerName">The controller name.</param>
    /// <param name="routeValues">The route values.</param>
    /// <param name="htmlAttributes">The HTML attributes.</param>
    /// <returns>The action link.</returns>
    public IHtmlContent ActionLink(string text, State state, string actionName, string controllerName, object routeValues = null, object htmlAttributes = null) => provider.ActionLink(html, text, state, actionName, controllerName, routeValues, htmlAttributes);

    /// <summary>
    /// Gets the button.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="state">The state.</param>
    /// <param name="onClick">The on click.</param>
    /// <param name="htmlAttributes">The HTML attributes.</param>
    /// <returns>The button.</returns>
    public IHtmlContent Button(string text, State state, string onClick = null, object htmlAttributes = null) => provider.Button(text, state, onClick, htmlAttributes);

    /// <summary>
    /// Submits the button.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="state">The state.</param>
    /// <param name="htmlAttributes">The HTML attributes.</param>
    /// <returns>The button.</returns>
    public IHtmlContent SubmitButton(string text, State state, object htmlAttributes = null) => provider.SubmitButton(text, state, htmlAttributes);

    #endregion Buttons

    #region Modal (Dialog)

    /// <summary>
    /// Begins the value.
    /// </summary>
    /// <param name="modal">The modal.</param>
    /// <returns>The t modal builder.</returns>
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

    /// <summary>
    /// Begins the value.
    /// </summary>
    /// <param name="panel">The panel.</param>
    /// <returns>The t panel builder.</returns>
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

    /// <summary>
    /// Begins the value.
    /// </summary>
    /// <param name="tabs">The tabs.</param>
    /// <returns>The t tabs builder.</returns>
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