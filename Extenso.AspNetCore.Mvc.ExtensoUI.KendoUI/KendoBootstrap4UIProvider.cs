using Extenso.AspNetCore.Mvc.ExtensoUI.Providers;

namespace Extenso.AspNetCore.Mvc.ExtensoUI.KendoUI;

/// <summary>
/// Represents the kendo bootstrap4ui provider.
/// </summary>
public class KendoBootstrap4UIProvider : Bootstrap4UIProvider
{
    private IAccordionProvider accordionProvider;
    private IModalProvider modalProvider;
    private ITabsProvider tabsProvider;

    #region IExtensoUIProvider Members

    /// <inheritdoc/>
    public override IAccordionProvider AccordionProvider => accordionProvider ??= new KendoUIAccordionProvider(this);

    /// <inheritdoc/>
    public override IModalProvider ModalProvider => modalProvider ??= new KendoUIModalProvider(this);

    /// <inheritdoc/>
    public override ITabsProvider TabsProvider => tabsProvider ??= new KendoUITabsProvider(this);

    #endregion IExtensoUIProvider Members

    /// <inheritdoc/>
    protected override string GetButtonCssClass(State state) => state switch
    {
        State.Primary => "k-primary k-button",
        _ => "k-button",
    };
}