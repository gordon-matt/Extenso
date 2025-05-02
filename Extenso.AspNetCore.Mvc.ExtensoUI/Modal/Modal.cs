using System.Diagnostics;

namespace Extenso.AspNetCore.Mvc.ExtensoUI;

[DebuggerDisplay("Id = {Id}")]
public class Modal : HtmlElement
{
    public string Id { get; private set; }

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

    protected internal override void StartTag(TextWriter textWriter) => Provider.ModalProvider.BeginModal(this, textWriter);

    protected internal override void EndTag(TextWriter textWriter) => Provider.ModalProvider.EndModal(this, textWriter);
}