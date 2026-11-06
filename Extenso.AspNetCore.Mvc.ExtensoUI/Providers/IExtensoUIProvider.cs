using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Extenso.AspNetCore.Mvc.ExtensoUI.Providers;

/// <summary>
/// Defines the contract for the extenso UI provider.
/// </summary>
public interface IExtensoUIProvider
{
    /// <summary>
    /// Renders the scripts.
    /// </summary>
    /// <returns>The scripts.</returns>
    IHtmlContent RenderScripts();

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
    IHtmlContent ActionLink(IHtmlHelper html, string text, State state, string actionName, string controllerName, object routeValues = null, object htmlAttributes = null);

    /// <summary>
    /// Gets the button.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="state">The state.</param>
    /// <param name="onClick">The on click.</param>
    /// <param name="htmlAttributes">The HTML attributes.</param>
    /// <returns>The button.</returns>
    IHtmlContent Button(string text, State state, string onClick = null, object htmlAttributes = null);

    /// <summary>
    /// Submits the button.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="state">The state.</param>
    /// <param name="htmlAttributes">The HTML attributes.</param>
    /// <returns>The button.</returns>
    IHtmlContent SubmitButton(string text, State state, object htmlAttributes = null);

    #endregion General

    #region Special

    /// <summary>
    /// Gets the accordion provider.
    /// </summary>
    IAccordionProvider AccordionProvider { get; }

    /// <summary>
    /// Gets the modal provider.
    /// </summary>
    IModalProvider ModalProvider { get; }

    /// <summary>
    /// Gets the panel provider.
    /// </summary>
    IPanelProvider PanelProvider { get; }

    /// <summary>
    /// Gets the tabs provider.
    /// </summary>
    ITabsProvider TabsProvider { get; }

    #endregion Special
}