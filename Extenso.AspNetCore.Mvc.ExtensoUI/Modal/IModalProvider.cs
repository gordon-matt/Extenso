using Microsoft.AspNetCore.Html;

namespace Extenso.AspNetCore.Mvc.ExtensoUI;

public interface IModalProvider
{
    void BeginModal(Modal modal, TextWriter writer);

    void BeginModalSectionPanel(ModalSection section, TextWriter writer, string title = null);

    void EndModal(Modal modal, TextWriter writer);

    void EndModalSectionPanel(ModalSection section, TextWriter writer);

    IHtmlContent ModalLaunchButton(string modalId, string text, object htmlAttributes = null);

    IHtmlContent ModalCloseButton(string modalId, string text, object htmlAttributes = null);
}