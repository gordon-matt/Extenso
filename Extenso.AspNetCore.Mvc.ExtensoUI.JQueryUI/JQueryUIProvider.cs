using Extenso.AspNetCore.Mvc.ExtensoUI.Providers;

namespace Extenso.AspNetCore.Mvc.ExtensoUI.JQueryUI;

/// <summary>
/// Represents the j query UI provider.
/// </summary>
public class JQueryUIProvider : BaseUIProvider
{
    private IAccordionProvider accordionProvider;
    private IModalProvider modalProvider;
    private IPanelProvider panelProvider;
    private ITabsProvider tabsProvider;

    #region IExtensoUIProvider Members

    /// <inheritdoc/>
    public override IAccordionProvider AccordionProvider => accordionProvider ??= new JQueryUIAccordionProvider(this);

    /// <inheritdoc/>
    public override IModalProvider ModalProvider => modalProvider ??= new JQueryUIModalProvider(this);

    /// <inheritdoc/>
    public override IPanelProvider PanelProvider => panelProvider ??= new JQueryUIPanelProvider();

    /// <inheritdoc/>
    public override ITabsProvider TabsProvider => tabsProvider ??= new JQueryUITabsProvider(this);

    #endregion IExtensoUIProvider Members

    /// <inheritdoc/>
    protected override string GetButtonCssClass(State state) => state switch
    {
        State.Primary => "k-primary k-button",
        _ => "k-button",
    };
}