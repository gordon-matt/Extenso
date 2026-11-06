using Extenso.AspNetCore.Mvc.ExtensoUI.Providers;
using Extenso.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Extenso.AspNetCore.Mvc.ExtensoUI.Foundation;

/// <summary>
/// Represents the foundation modal provider.
/// </summary>
public class FoundationModalProvider : IModalProvider
{
    private readonly BaseUIProvider uiProvider;

    /// <summary>
    /// Initializes a new instance of the FoundationModalProvider class.
    /// </summary>
    /// <param name="uiProvider">The UI provider.</param>
    public FoundationModalProvider(BaseUIProvider uiProvider)
    {
        this.uiProvider = uiProvider;
    }

    #region IModalProvider Members

    /// <summary>
    /// Begins the modal.
    /// </summary>
    /// <param name="modal">The modal.</param>
    /// <param name="writer">The writer.</param>
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
    public void EndModal(Modal modal, TextWriter writer) => writer.Write("</div></div>");

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
            .AddCssClass("button primary")
            .MergeAttribute("type", "button")
            .MergeAttribute("data-open", $"{modalId}")
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
            .AddCssClass("button secondary")
            .MergeAttribute("type", "button")
            .MergeAttribute("onclick", $"function() {{ $('#{modalId}').foundation('close'); }}")
            .SetInnerHtml(text);

        return new HtmlString(builder.ToString());
    }

    #endregion IModalProvider Members
}