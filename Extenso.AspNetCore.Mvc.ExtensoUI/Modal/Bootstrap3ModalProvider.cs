using Extenso.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Extenso.AspNetCore.Mvc.ExtensoUI;

/// <summary>
/// Represents the bootstrap3modal provider.
/// </summary>
public class Bootstrap3ModalProvider : IModalProvider
{
    #region IModalProvider Members

    /// <summary>
    /// Begins the modal.
    /// </summary>
    /// <param name="modal">The modal.</param>
    /// <param name="writer">The writer.</param>
    public void BeginModal(Modal modal, TextWriter writer)
    {
        modal.EnsureClass("modal fade");
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
                    writer.Write(@"<div class=""modal-dialog""><div class=""modal-content"">");

                    //var builder = new FluentTagBuilder("div", TagRenderMode.StartTag)
                    //    .AddCssClass("modal-dialog")
                    //    .StartTag("div", TagRenderMode.StartTag)
                    //        .AddCssClass("modal-content")
                    //    .EndTag();

                    //writer.Write(builder.ToString());

                    writer.Write(@"<div class=""modal-header"">");

                    writer.Write(
$@"<button type=""button"" class=""close"" data-dismiss=""modal"" aria-label=""Close"">
    <span aria-hidden=""true"">&times;</span>
</button>
<h4 class=""modal-title"">{title}</h4>");
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
    public void EndModalSectionPanel(ModalSection section, TextWriter writer) =>
        //if (section == ModalSection.Footer)
        //{
        //    writer.Write("</div></div></div>");
        //}
        //else
        //{
        //    writer.Write("</div>");
        //}
        writer.Write("</div>");

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
            .AddCssClass("btn btn-primary")
            .MergeAttribute("type", "button")
            .MergeAttribute("data-toggle", "modal")
            .MergeAttribute("data-target", $"#{modalId}")
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
            .AddCssClass("btn btn-default")
            .MergeAttribute("type", "button")
            .MergeAttribute("data-dismiss", "modal")
            .SetInnerHtml(text);

        return new HtmlString(builder.ToString());
    }

    #endregion IModalProvider Members
}