using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Extenso.AspNetCore.Mvc.ExtensoUI.Providers
{
    public interface IExtensoUIProvider
    {
        IHtmlContent RenderScripts();

        #region General

        IHtmlContent ActionLink(IHtmlHelper html, string text, State state, string actionName, string controllerName, object routeValues = null, object htmlAttributes = null);

        IHtmlContent Button(string text, State state, string onClick = null, object htmlAttributes = null);

        IHtmlContent SubmitButton(string text, State state, object htmlAttributes = null);

        #endregion General

        #region Special

        IAccordionProvider AccordionProvider { get; }

        IModalProvider ModalProvider { get; }

        IPanelProvider PanelProvider { get; }

        ITabsProvider TabsProvider { get; }

        #endregion Special
    }
}