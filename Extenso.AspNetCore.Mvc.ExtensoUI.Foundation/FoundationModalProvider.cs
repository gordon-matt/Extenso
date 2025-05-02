using Extenso.AspNetCore.Mvc.ExtensoUI.Providers;
using Extenso.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Extenso.AspNetCore.Mvc.ExtensoUI.Foundation;

public class FoundationModalProvider : IModalProvider
{
    private readonly BaseUIProvider uiProvider;

    public FoundationModalProvider(BaseUIProvider uiProvider)
    {
        this.uiProvider = uiProvider;
    }

    #region IModalProvider Members

    public void BeginModal(Modal modal, TextWriter writer)
    {
        modal.EnsureClass("reveal");
        modal.EnsureHtmlAttribute("id", modal.Id);
        modal.EnsureHtmlAttribute("data-reveal", string.Empty);

        var builder = new FluentTagBuilder("div", TagRenderMode.StartTag)
            .MergeAttributes(modal.HtmlAttributes);

        string tag = builder.ToString();

        writer.Write(tag);
    }

    public void BeginModalSectionPanel(ModalSection section, TextWriter writer, string title = null)
    {
        switch (section)
        {
            case ModalSection.Header:
                {
                    writer.Write(@"<div class=""modal-header"">");

                    writer.Write(
$@"<button type=""button"" class=""close-button"" data-close aria-label=""Close"">
    <span aria-hidden=""true"">&times;</span>
</button>
<h1 class=""modal-title"">{title}</h1>");
                }
                break;

            case ModalSection.Body: writer.Write(@"<div class=""modal-body"">"); break;
            case ModalSection.Footer: writer.Write(@"<div class=""modal-footer"">"); break;
        }
    }

    public void EndModalSectionPanel(ModalSection section, TextWriter writer) => writer.Write("</div>");

    public void EndModal(Modal modal, TextWriter writer) => writer.Write("</div></div>");

    public IHtmlContent ModalLaunchButton(string modalId, string text, object htmlAttributes = null)
    {
        var builder = new FluentTagBuilder("button")
            .AddCssClass("button primary")
            .MergeAttribute("type", "button")
            .MergeAttribute("data-open", $"{modalId}")
            .SetInnerHtml(text);

        return new HtmlString(builder.ToString());
    }

    public IHtmlContent ModalCloseButton(string modalId, string text, object htmlAttributes = null)
    {
        var builder = new FluentTagBuilder("button")
            .AddCssClass("button secondary")
            .MergeAttribute("type", "button")
            .MergeAttribute("onclick", $"function() {{ $('#{modalId}').foundation('close'); }}")
            .SetInnerHtml(text);

        return new HtmlString(builder.ToString());
    }

    #endregion IModalProvider Members
}