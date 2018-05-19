using System;
using System.IO;

namespace Extenso.AspNetCore.Mvc.ExtensoUI
{
    public class Accordion : HtmlElement
    {
        public string Id { get; private set; }

        public Accordion(string id = null, object htmlAttributes = null)
            : base(htmlAttributes)
        {
            if (string.IsNullOrEmpty(id))
            {
                id = $"accordion-{Guid.NewGuid()}";
            }

            Id = id;
            EnsureHtmlAttribute("id", Id);
        }

        protected internal override void StartTag(TextWriter textWriter)
        {
            Provider.AccordionProvider.BeginAccordion(this, textWriter);
        }

        protected internal override void EndTag(TextWriter textWriter)
        {
            Provider.AccordionProvider.EndAccordion(this, textWriter);
        }
    }
}