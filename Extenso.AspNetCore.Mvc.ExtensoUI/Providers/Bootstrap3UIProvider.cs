namespace Extenso.AspNetCore.Mvc.ExtensoUI.Providers
{
    public class Bootstrap3UIProvider : BaseUIProvider
    {
        private IAccordionProvider accordionProvider;
        private IModalProvider modalProvider;
        private IPanelProvider panelProvider;
        private ITabsProvider tabsProvider;

        #region IExtensoUIProvider Members

        #region Special

        public override IAccordionProvider AccordionProvider =>
            accordionProvider ?? (accordionProvider = new Bootstrap3AccordionProvider());

        public override IModalProvider ModalProvider =>
            modalProvider ?? (modalProvider = new Bootstrap3ModalProvider());

        public override IPanelProvider PanelProvider =>
            panelProvider ?? (panelProvider = new Bootstrap3PanelProvider());

        public override ITabsProvider TabsProvider =>
            tabsProvider ?? (tabsProvider = new Bootstrap3TabsProvider());

        #endregion Special

        #endregion IExtensoUIProvider Members

        protected override string GetButtonCssClass(State state)
        {
            switch (state)
            {
                case State.Primary: return "btn btn-primary";
                case State.Secondary: return "btn btn-default";
                case State.Success: return "btn btn-success";
                case State.Danger: return "btn btn-danger";
                case State.Warning: return "btn btn-warning";
                case State.Info: return "btn btn-info";
                case State.Dark: return "btn btn-inverse";
                case State.Light: return "btn btn-default";
                default: return "btn btn-default";
            }
        }
    }
}