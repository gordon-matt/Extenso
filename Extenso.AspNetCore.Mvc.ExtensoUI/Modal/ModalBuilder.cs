using Microsoft.AspNetCore.Mvc.Rendering;

namespace Extenso.AspNetCore.Mvc.ExtensoUI
{
    public class ModalBuilder<TModel> : BuilderBase<TModel, Modal>
    {
        internal ModalBuilder(IHtmlHelper<TModel> htmlHelper, Modal modal)
            : base(htmlHelper, modal)
        {
        }

        public ModalSectionPanel BeginHeader(string title)
        {
            return new ModalSectionPanel(Element.Provider, ModalSection.Header, TextWriter, title);
        }

        public ModalSectionPanel BeginBody()
        {
            return new ModalSectionPanel(Element.Provider, ModalSection.Body, TextWriter);
        }

        public ModalSectionPanel BeginFooter()
        {
            return new ModalSectionPanel(Element.Provider, ModalSection.Footer, TextWriter);
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}