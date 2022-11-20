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
            return state switch
            {
                State.Danger => "btn btn-danger",
                State.Default => "btn btn-default",
                State.Info => "btn btn-info",
                State.Inverse => "btn btn-inverse",
                State.Primary => "btn btn-primary",
                State.Success => "btn btn-success",
                State.Warning => "btn btn-warning",
                _ => "btn btn-default",
            };
        }
    }
}