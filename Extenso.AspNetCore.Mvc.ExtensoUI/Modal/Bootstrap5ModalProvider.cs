using Extenso.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Extenso.AspNetCore.Mvc.ExtensoUI;

/// <summary>
/// Represents the bootstrap5modal provider.
/// </summary>
public class Bootstrap5ModalProvider : IModalProvider
{
    #region IModalProvider Members

    /// <summary>
    /// Begins the modal.
    /// </summary>
    /// <param name="modal">The modal.</param>
    /// <param name="writer">The writer.</param>
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

    /// <summary>
    /// Begins the modal section panel.
    /// </summary>
    /// <param name="section">The section.</param>
    /// <param name="writer">The writer.</param>
    /// <param name="title">The title.</param>
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
<button type=""button"" class=""btn-close"" data-bs-dismiss=""modal"" aria-label=""Close""></button>");
                }
                break;

            case ModalSection.Body: writer.Write(@"<div class=""modal-body"">"); break;
            case ModalSection.Footer: writer.Write(@"<div class=""modal-footer"">"); break;
        }
    }

    /// <summary>
    /// Performs the end modal section panel operation.
    /// </summary>
    /// <param name="section">The section.</param>
    /// <param name="writer">The writer.</param>
    public void EndModalSectionPanel(ModalSection section, TextWriter writer) => writer.Write("</div>");

    /// <summary>
    /// Performs the end modal operation.
    /// </summary>
    /// <param name="modal">The modal.</param>
    /// <param name="writer">The writer.</param>
    public void EndModal(Modal modal, TextWriter writer) =>
        //writer.Write("</div>");
        writer.Write("</div></div></div>");

    /// <summary>
    /// Gets the modal launch button.
    /// </summary>
    /// <param name="modalId">The modal identifier.</param>
    /// <param name="text">The text.</param>
    /// <param name="htmlAttributes">The HTML attributes.</param>
    /// <returns>The modal launch button.</returns>
    public IHtmlContent ModalLaunchButton(string modalId, string text, object htmlAttributes = null)
    {
        var builder = new FluentTagBuilder("button")
            .MergeAttribute("type", "button")
            .AddCssClass("btn btn-primary")
            .MergeAttribute("data-bs-toggle", "modal")
            .MergeAttribute("data-bs-target", $"#{modalId}")
            .SetInnerHtml(text);

        return new HtmlString(builder.ToString());
    }

    /// <summary>
    /// Gets the modal close button.
    /// </summary>
    /// <param name="modalId">The modal identifier.</param>
    /// <param name="text">The text.</param>
    /// <param name="htmlAttributes">The HTML attributes.</param>
    /// <returns>The modal close button.</returns>
    public IHtmlContent ModalCloseButton(string modalId, string text, object htmlAttributes = null)
    {
        var builder = new FluentTagBuilder("button")
            .MergeAttribute("type", "button")
            .AddCssClass("btn btn-secondary")
            .MergeAttribute("data-bs-dismiss", "modal")
            .SetInnerHtml(text);

        return new HtmlString(builder.ToString());
    }

    #endregion IModalProvider Members
}