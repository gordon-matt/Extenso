using Microsoft.AspNetCore.Mvc.Rendering;

namespace Extenso.AspNetCore.Mvc.ExtensoUI;

public class PanelBuilder<TModel> : BuilderBase<TModel, Panel>
{
    internal PanelBuilder(IHtmlHelper<TModel> htmlHelper, Panel panel)
        : base(htmlHelper, panel)
    {
    }

    // TODO: Support html attributes
    public PanelSection BeginHeader(string title) => new(Element.Provider, PanelSectionType.Heading, TextWriter, title);

    public PanelSection BeginBody() => new(Element.Provider, PanelSectionType.Body, TextWriter);

    public PanelSection BeginFooter() => new(Element.Provider, PanelSectionType.Footer, TextWriter);

    public override void Dispose() => base.Dispose();
}