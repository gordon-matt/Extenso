using Extenso.AspNetCore.Mvc.ExtensoUI.Providers;

namespace Extenso.AspNetCore.Mvc.ExtensoUI.Foundation
{
    public class Foundation6UIProvider : BaseUIProvider
    {
        private IAccordionProvider accordionProvider;
        private IModalProvider modalProvider;
        private IPanelProvider panelProvider;
        private ITabsProvider tabsProvider;

        #region IExtensoUIProvider Members

        public override IAccordionProvider AccordionProvider =>
            accordionProvider ?? (accordionProvider = new FoundationAccordionProvider());

        public override IModalProvider ModalProvider =>
            modalProvider ?? (modalProvider = new FoundationModalProvider(this));

        public override IPanelProvider PanelProvider =>
            panelProvider ?? (panelProvider = new FoundationPanelProvider());

        public override ITabsProvider TabsProvider =>
            tabsProvider ?? (tabsProvider = new FoundationTabsProvider());

        #endregion IExtensoUIProvider Members

        protected override string GetButtonCssClass(State state)
        {
            switch (state)
            {
                case State.Primary: return "button primary";
                case State.Secondary: return "button secondary";
                case State.Success: return "button success";
                case State.Danger: return "button alert";
                case State.Warning: return "button warning";
                case State.Info: return "button primary";
                case State.Light: return "button secondary";
                case State.Dark: return "button primary";
                default: return "button";
            }
        }
    }
}