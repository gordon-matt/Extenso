using Microsoft.AspNetCore.Html;

namespace Extenso.AspNetCore.Mvc.ExtensoUI;

/// <summary>
/// Defines the contract for the modal provider.
/// </summary>
public interface IModalProvider
{
    /// <summary>
    /// Begins the modal.
    /// </summary>
    /// <param name="modal">The modal.</param>
    /// <param name="writer">The writer.</param>
    void BeginModal(Modal modal, TextWriter writer);

    /// <summary>
    /// Begins the modal section panel.
    /// </summary>
    /// <param name="section">The section.</param>
    /// <param name="writer">The writer.</param>
    /// <param name="title">The title.</param>
    void BeginModalSectionPanel(ModalSection section, TextWriter writer, string title = null);

    /// <summary>
    /// Performs the end modal operation.
    /// </summary>
    /// <param name="modal">The modal.</param>
    /// <param name="writer">The writer.</param>
    void EndModal(Modal modal, TextWriter writer);

    /// <summary>
    /// Performs the end modal section panel operation.
    /// </summary>
    /// <param name="section">The section.</param>
    /// <param name="writer">The writer.</param>
    void EndModalSectionPanel(ModalSection section, TextWriter writer);

    /// <summary>
    /// Gets the modal launch button.
    /// </summary>
    /// <param name="modalId">The modal identifier.</param>
    /// <param name="text">The text.</param>
    /// <param name="htmlAttributes">The HTML attributes.</param>
    /// <returns>The modal launch button.</returns>
    IHtmlContent ModalLaunchButton(string modalId, string text, object htmlAttributes = null);

    /// <summary>
    /// Gets the modal close button.
    /// </summary>
    /// <param name="modalId">The modal identifier.</param>
    /// <param name="text">The text.</param>
    /// <param name="htmlAttributes">The HTML attributes.</param>
    /// <returns>The modal close button.</returns>
    IHtmlContent ModalCloseButton(string modalId, string text, object htmlAttributes = null);
}