using Extenso.AspNetCore.Mvc.ExtensoUI.Providers;

namespace Extenso.AspNetCore.Mvc.ExtensoUI.Foundation;

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

    protected override string GetButtonCssClass(State state) => state switch
    {
        State.Danger => "button alert",
        State.Default => "button secondary",
        State.Info => "button primary",
        State.Inverse => "button secondary",
        State.Primary => "button primary",
        State.Success => "button success",
        State.Warning => "button warning",
        _ => "button",
    };
}