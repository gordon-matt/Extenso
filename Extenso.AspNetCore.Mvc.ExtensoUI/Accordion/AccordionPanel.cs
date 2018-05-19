using System;
using System.IO;
using Extenso.AspNetCore.Mvc.ExtensoUI.Providers;

namespace Extenso.AspNetCore.Mvc.ExtensoUI
{
    public class AccordionPanel : IDisposable
    {
        private readonly TextWriter textWriter;
        private readonly IExtensoUIProvider provider;

        internal AccordionPanel(IExtensoUIProvider provider, TextWriter writer, string title, string panelId, string parentAccordionId, bool expanded = false)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentNullException(nameof(title));
            }

            this.provider = provider;
            this.textWriter = writer;

            provider.AccordionProvider.BeginAccordionPanel(textWriter, title, panelId, parentAccordionId, expanded);
        }

        public void Dispose()
        {
            provider.AccordionProvider.EndAccordionPanel(textWriter);
        }
    }
}