namespace Extenso.AspNetCore.Mvc.ExtensoUI.Providers;

/// <summary>
/// Represents the bootstrap3ui provider.
/// </summary>
public class Bootstrap3UIProvider : BaseUIProvider
{
    private IAccordionProvider accordionProvider;
    private IModalProvider modalProvider;
    private IPanelProvider panelProvider;
    private ITabsProvider tabsProvider;

    #region IExtensoUIProvider Members

    #region Special

    /// <inheritdoc/>
    public override IAccordionProvider AccordionProvider => accordionProvider ??= new Bootstrap3AccordionProvider();

    /// <inheritdoc/>
    public override IModalProvider ModalProvider => modalProvider ??= new Bootstrap3ModalProvider();

    /// <inheritdoc/>
    public override IPanelProvider PanelProvider => panelProvider ??= new Bootstrap3PanelProvider();

    /// <inheritdoc/>
    public override ITabsProvider TabsProvider => tabsProvider ??= new Bootstrap3TabsProvider();

    #endregion Special

    #endregion IExtensoUIProvider Members

    /// <inheritdoc/>
    protected override string GetButtonCssClass(State state) => state switch
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