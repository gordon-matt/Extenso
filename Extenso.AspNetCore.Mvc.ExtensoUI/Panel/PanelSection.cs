using System;
using System.IO;
using Extenso.AspNetCore.Mvc.ExtensoUI.Providers;

namespace Extenso.AspNetCore.Mvc.ExtensoUI
{
    public enum PanelSectionType : byte
    {
        Heading,
        Body,
        Footer
    }

    public class PanelSection : IDisposable
    {
        private readonly TextWriter textWriter;
        private readonly IExtensoUIProvider provider;

        public PanelSectionType SectionType { get; private set; }

        // TODO: Support html attributes
        internal PanelSection(IExtensoUIProvider provider, PanelSectionType sectionType, TextWriter writer, string title = null)
        {
            this.provider = provider;
            SectionType = sectionType;
            textWriter = writer;
            provider.PanelProvider.BeginPanelSection(SectionType, textWriter, title);
        }

        public void Dispose()
        {
            provider.PanelProvider.EndPanelSection(SectionType, textWriter);
        }
    }
}