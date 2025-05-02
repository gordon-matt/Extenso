using Extenso.AspNetCore.Mvc.ExtensoUI.Providers;
using Microsoft.AspNetCore.Html;

namespace Extenso.AspNetCore.Mvc.ExtensoUI;

public enum ModalSection
{
    Header,
    Body,
    Footer
}

public class ModalSectionPanel : IDisposable
{
    private readonly TextWriter textWriter;
    private readonly IExtensoUIProvider provider;

    public ModalSection Section { get; private set; }

    internal ModalSectionPanel(IExtensoUIProvider provider, ModalSection section, TextWriter writer, string title = null)
    {
        this.provider = provider;
        Section = section;
        textWriter = writer;
        provider.ModalProvider.BeginModalSectionPanel(Section, textWriter, title);
    }

    public IHtmlContent ModalCloseButton(string modalId, string text, object htmlAttributes = null) => provider.ModalProvider.ModalCloseButton(modalId, text, htmlAttributes);

    public void Dispose() => provider.ModalProvider.EndModalSectionPanel(Section, textWriter);
}