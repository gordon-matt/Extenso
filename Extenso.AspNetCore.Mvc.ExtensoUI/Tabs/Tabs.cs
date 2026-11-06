using System.Diagnostics;

namespace Extenso.AspNetCore.Mvc.ExtensoUI;

/// <summary>
/// Represents the tabs.
/// </summary>
[DebuggerDisplay("Id = {Id}")]
public class Tabs : HtmlElement
{
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    public string Id { get; private set; }

    /// <summary>
    /// Initializes a new instance of the Tabs class.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="htmlAttributes">The HTML attributes.</param>
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

    /// <inheritdoc/>
    protected internal override void StartTag(TextWriter textWriter) => Provider.TabsProvider.BeginTabs(this, textWriter);

    /// <inheritdoc/>
    protected internal override void EndTag(TextWriter textWriter) => Provider.TabsProvider.EndTabs(this, textWriter);
}