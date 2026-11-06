using System.Diagnostics;

namespace Extenso.AspNetCore.Mvc.ExtensoUI;

/// <summary>
/// Represents the modal.
/// </summary>
[DebuggerDisplay("Id = {Id}")]
public class Modal : HtmlElement
{
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    public string Id { get; private set; }

    /// <summary>
    /// Initializes a new instance of the Modal class.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="htmlAttributes">The HTML attributes.</param>
    public Modal(string id = null, object htmlAttributes = null)
        : base(htmlAttributes)
    {
        if (string.IsNullOrEmpty(id))
        {
            id = $"modal-{Guid.NewGuid()}";
        }
        Id = id;
        EnsureHtmlAttribute("id", Id);
    }

    /// <inheritdoc/>
    protected internal override void StartTag(TextWriter textWriter) => Provider.ModalProvider.BeginModal(this, textWriter);

    /// <inheritdoc/>
    protected internal override void EndTag(TextWriter textWriter) => Provider.ModalProvider.EndModal(this, textWriter);
}