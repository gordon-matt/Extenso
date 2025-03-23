using Microsoft.AspNetCore.Mvc.Rendering;

namespace Extenso.AspNetCore.Mvc.ExtensoUI;

public class ModalBuilder<TModel> : BuilderBase<TModel, Modal>
{
    internal ModalBuilder(IHtmlHelper<TModel> htmlHelper, Modal modal)
        : base(htmlHelper, modal)
    {
    }

    public ModalSectionPanel BeginHeader(string title) => new(Element.Provider, ModalSection.Header, TextWriter, title);

    public ModalSectionPanel BeginBody() => new(Element.Provider, ModalSection.Body, TextWriter);

    public ModalSectionPanel BeginFooter() => new(Element.Provider, ModalSection.Footer, TextWriter);

    public override void Dispose() => base.Dispose();
}