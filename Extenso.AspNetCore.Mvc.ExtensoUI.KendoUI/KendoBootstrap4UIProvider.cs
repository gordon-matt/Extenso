using Extenso.AspNetCore.Mvc.ExtensoUI.Providers;

namespace Extenso.AspNetCore.Mvc.ExtensoUI.KendoUI
{
    public class KendoBootstrap4UIProvider : Bootstrap4UIProvider
    {
        private IAccordionProvider accordionProvider;
        private IModalProvider modalProvider;
        private ITabsProvider tabsProvider;

        #region IExtensoUIProvider Members

        public override IAccordionProvider AccordionProvider => accordionProvider ??= new KendoUIAccordionProvider(this);

        public override IModalProvider ModalProvider => modalProvider ??= new KendoUIModalProvider(this);

        public override ITabsProvider TabsProvider => tabsProvider ??= new KendoUITabsProvider(this);

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