using Microsoft.AspNetCore.Mvc.Rendering;

namespace Extenso.AspNetCore.Mvc.ExtensoUI
{
    public class AccordionBuilder<TModel> : BuilderBase<TModel, Accordion>
    {
        internal AccordionBuilder(IHtmlHelper<TModel> htmlHelper, Accordion accordion)
            : base(htmlHelper, accordion)
        {
        }

        public AccordionPanel BeginPanel(string title, string id, bool expanded = false)
        {
            return new AccordionPanel(Element.Provider, TextWriter, title, id, Element.Id, expanded);
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}