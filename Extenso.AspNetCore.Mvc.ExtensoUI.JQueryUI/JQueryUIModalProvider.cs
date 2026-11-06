using Extenso.AspNetCore.Mvc.ExtensoUI.Providers;
using Extenso.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Extenso.AspNetCore.Mvc.ExtensoUI.JQueryUI;

/// <summary>
/// Represents the j query UI modal provider.
/// </summary>
public class JQueryUIModalProvider : IModalProvider
{
    private string modalId;
    private readonly BaseUIProvider uiProvider;

    /// <summary>
    /// Initializes a new instance of the JQueryUIModalProvider class.
    /// </summary>
    /// <param name="uiProvider">The UI provider.</param>
    public JQueryUIModalProvider(BaseUIProvider uiProvider)
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
        modalId = modal.Id;

        uiProvider.Scripts.Add(
$@"$(""#{modalId}"").dialog({{
    modal: true
}});");

        modal.EnsureHtmlAttribute("title", "Modal Dialog"); // this will be replaced in script...

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
        if (!string.IsNullOrEmpty(title))
        {
            uiProvider.Scripts.Add($"$('#{modalId}').attr('title', '{title}')");
        }

        switch (section)
        {
            case ModalSection.Header:
                {
                    writer.Write(@"<div class=""modal-header"">");
                }
                break;

            case ModalSection.Body: writer.Write(@"<div class=""modal-body"">"); break;
            case ModalSection.Footer: writer.Write(@"<div class=""modal-footer"">"); break;
        }
    }

    /// <summary>
    /// Performs the end modal operation.
    /// </summary>
    /// <param name="modal">The modal.</param>
    /// <param name="writer">The writer.</param>
    public void EndModal(Modal modal, TextWriter writer) => writer.Write("</div>");

    /// <summary>
    /// Performs the end modal section panel operation.
    /// </summary>
    /// <param name="section">The section.</param>
    /// <param name="writer">The writer.</param>
    public void EndModalSectionPanel(ModalSection section, TextWriter writer) => writer.Write("</div>");

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
            .GenerateId($"{modalId}-launch-button")
            .AddCssClass("ui-button ui-widget ui-corner-all")
            .SetInnerHtml(text);

        uiProvider.Scripts.Add(
$@"$('#{modalId}-launch-button')
.bind('click', function() {{
    $('#{modalId}').dialog('open');
}});");

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
            .GenerateId($"{modalId}-close-button")
            .AddCssClass("ui-button ui-widget ui-corner-all")
            .SetInnerHtml(text);

        uiProvider.Scripts.Add(
$@"$('#{modalId}-close-button')
.bind('click', function() {{
    $('#{modalId}').dialog('close');
}});");

        return new HtmlString(builder.ToString());
    }

    #endregion IModalProvider Members
}