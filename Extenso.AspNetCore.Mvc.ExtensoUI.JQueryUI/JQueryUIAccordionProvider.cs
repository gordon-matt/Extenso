using System.IO;
using Extenso.AspNetCore.Mvc.ExtensoUI.Providers;
using Extenso.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Extenso.AspNetCore.Mvc.ExtensoUI.JQueryUI
{
    public class JQueryUIAccordionProvider : IAccordionProvider
    {
        private readonly BaseUIProvider uiProvider;

        public JQueryUIAccordionProvider(BaseUIProvider uiProvider)
        {
            this.uiProvider = uiProvider;
        }

        #region IAccordionProvider Members

        public void BeginAccordion(Accordion accordion, TextWriter writer)
        {
            uiProvider.Scripts.Add($@"$('#{accordion.Id}').accordion();");

            var builder = new FluentTagBuilder("div", TagRenderMode.StartTag)
                .MergeAttributes(accordion.HtmlAttributes);

            string tag = builder.ToString();

            writer.Write(tag);
        }

        public void BeginAccordionPanel(TextWriter writer, string title, string panelId, string parentAccordionId, bool expanded)
        {
            writer.Write($"<h3>{title}</h3><div>");
        }

        public void EndAccordionPanel(TextWriter writer)
        {
            writer.Write("</div>");
        }

        public void EndAccordion(Accordion accordion, TextWriter writer)
        {
            writer.Write("</div>");
        }

        #endregion IAccordionProvider Members
    }
}