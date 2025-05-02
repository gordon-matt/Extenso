using System.Diagnostics;

namespace Extenso.AspNetCore.Mvc.ExtensoUI;

[DebuggerDisplay("Id = {Id}")]
public class Tabs : HtmlElement
{
    public string Id { get; private set; }

    public Tabs(string id = null, object htmlAttributes = null)
        : base(htmlAttributes)
    {
        if (string.IsNullOrEmpty(id))
        {
            id = $"tabs-{Guid.NewGuid()}";
        }

        Id = id;
        EnsureHtmlAttribute("id", Id);
    }

    protected internal override void StartTag(TextWriter textWriter) => Provider.TabsProvider.BeginTabs(this, textWriter);

    protected internal override void EndTag(TextWriter textWriter) => Provider.TabsProvider.EndTabs(this, textWriter);
}