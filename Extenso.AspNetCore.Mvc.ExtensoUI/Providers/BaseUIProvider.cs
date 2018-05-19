using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Extenso.AspNetCore.Mvc.ExtensoUI.Providers
{
    public abstract class BaseUIProvider : IExtensoUIProvider
    {
        private ICollection<string> scripts;

        public ICollection<string> Scripts
        {
            get { return scripts ?? (scripts = new List<string>()); }
            set { scripts = value; }
        }

        #region IExtensoUIProvider Members

        public virtual IHtmlContent RenderScripts()
        {
            var result = new HtmlString(string.Join(Environment.NewLine, Scripts));
            Scripts.Clear();
            return result;
        }

        #region General

        public abstract IHtmlContent ActionLink(IHtmlHelper html, string text, State state, string actionName, string controllerName, object routeValues = null, object htmlAttributes = null);

        public abstract IHtmlContent Badge(string text, object htmlAttributes = null);

        public abstract IHtmlContent Button(string text, State state, string onClick = null, object htmlAttributes = null);

        public abstract IHtmlContent InlineLabel(string text, State state, object htmlAttributes = null);

        public abstract IHtmlContent Quote(string text, string author, string titleOfWork, object htmlAttributes = null);

        public abstract IHtmlContent SubmitButton(string text, State state, object htmlAttributes = null);

        public abstract IHtmlContent TextBoxWithAddOns(IHtmlHelper html, string name, object value, string prependValue, string appendValue, object htmlAttributes = null);

        #endregion General

        #region Special

        public abstract IAccordionProvider AccordionProvider { get; }

        public abstract IModalProvider ModalProvider { get; }

        public abstract IPanelProvider PanelProvider { get; }

        public abstract ITabsProvider TabsProvider { get; }

        #endregion Special

        #endregion IExtensoUIProvider Members

        protected abstract string GetButtonCssClass(State state);

        protected abstract string GetLabelCssClass(State state);
    }
}