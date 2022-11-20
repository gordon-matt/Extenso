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

        public override IAccordionProvider AccordionProvider => accordionProvider ??= new FoundationAccordionProvider();

        public override IModalProvider ModalProvider => modalProvider ??= new FoundationModalProvider(this);

        public override IPanelProvider PanelProvider => panelProvider ??= new FoundationPanelProvider();

        public override ITabsProvider TabsProvider => tabsProvider ??= new FoundationTabsProvider();

        #endregion IExtensoUIProvider Members

        protected override string GetButtonCssClass(State state)
        {
            switch (state)
            {
                case State.Danger: return "button alert";
                case State.Default: return "button secondary";
                case State.Info: return "button primary";
                case State.Inverse: return "button secondary";
                case State.Primary: return "button primary";
                case State.Success: return "button success";
                case State.Warning: return "button warning";
                default: return "button";
            }
        }
    }
}