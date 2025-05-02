using Extenso.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Extenso.AspNetCore.Mvc.ExtensoUI;

public class Bootstrap4ModalProvider : IModalProvider
{
    #region IModalProvider Members

    public void BeginModal(Modal modal, TextWriter writer)
    {
        modal.EnsureClass("modal");
        modal.EnsureHtmlAttribute("tabindex", "-1");
        modal.EnsureHtmlAttribute("role", "dialog");
        modal.EnsureHtmlAttribute("aria-labelledby", $"{modal.Id}-label");
        modal.EnsureHtmlAttribute("aria-hidden", "true");

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
                    writer.Write(@"<div class=""modal-dialog"" role=""document""><div class=""modal-content"">");

                    writer.Write(@"<div class=""modal-header"">");

                    writer.Write(
$@"<h5 class=""modal-title"">{title}</h5>
<button type=""button"" class=""close"" data-dismiss=""modal"" aria-label=""Close"">
    <span aria-hidden=""true"">&times;</span>
</button>");
                }
                break;

            case ModalSection.Body: writer.Write(@"<div class=""modal-body"">"); break;
            case ModalSection.Footer: writer.Write(@"<div class=""modal-footer"">"); break;
        }
    }

    public void EndModalSectionPanel(ModalSection section, TextWriter writer) => writer.Write("</div>");

    public void EndModal(Modal modal, TextWriter writer) =>
        //writer.Write("</div>");
        writer.Write("</div></div></div>");

    public IHtmlContent ModalLaunchButton(string modalId, string text, object htmlAttributes = null)
    {
        var builder = new FluentTagBuilder("button")
            .MergeAttribute("type", "button")
            .AddCssClass("btn btn-primary")
            .MergeAttribute("data-toggle", "modal")
            .MergeAttribute("data-target", $"#{modalId}")
            .SetInnerHtml(text);

        return new HtmlString(builder.ToString());
    }

    public IHtmlContent ModalCloseButton(string modalId, string text, object htmlAttributes = null)
    {
        var builder = new FluentTagBuilder("button")
            .MergeAttribute("type", "button")
            .AddCssClass("btn btn-secondary")
            .MergeAttribute("data-dismiss", "modal")
            .SetInnerHtml(text);

        return new HtmlString(builder.ToString());
    }

    #endregion IModalProvider Members
}