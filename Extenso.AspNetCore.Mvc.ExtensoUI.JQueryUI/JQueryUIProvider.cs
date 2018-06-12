using Extenso.AspNetCore.Mvc.ExtensoUI.Providers;

namespace Extenso.AspNetCore.Mvc.ExtensoUI.JQueryUI
{
    public class JQueryUIProvider : BaseUIProvider
    {
        private IAccordionProvider accordionProvider;
        private IModalProvider modalProvider;
        private IPanelProvider panelProvider;
        private ITabsProvider tabsProvider;

        #region IExtensoUIProvider Members

        public override IAccordionProvider AccordionProvider =>
            accordionProvider ?? (accordionProvider = new JQueryUIAccordionProvider(this));

        public override IModalProvider ModalProvider =>
            modalProvider ?? (modalProvider = new JQueryUIModalProvider(this));

        public override IPanelProvider PanelProvider =>
            panelProvider ?? (panelProvider = new JQueryUIPanelProvider());

        public override ITabsProvider TabsProvider =>
            tabsProvider ?? (tabsProvider = new JQueryUITabsProvider(this));

        #endregion IExtensoUIProvider Members

        protected override string GetButtonCssClass(State state)
        {
            switch (state)
            {
                case State.Primary: return "k-primary k-button";
                default: return "k-button";
            }
        }
    }
}