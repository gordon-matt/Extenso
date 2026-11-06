using System.Diagnostics;

namespace Extenso.AspNetCore.Mvc.ExtensoUI;

/// <summary>
/// Represents the panel.
/// </summary>
[DebuggerDisplay("Id = {Id}")]
public class Panel : HtmlElement
{
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    public string Id { get; private set; }

    /// <summary>
    /// Gets or sets the state.
    /// </summary>
    public State State { get; private set; }

    /// <summary>
    /// Initializes a new instance of the Panel class.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="state">The state.</param>
    /// <param name="htmlAttributes">The HTML attributes.</param>
    public Panel(string id = null, State state = State.Primary, object htmlAttributes = null)
        : base(htmlAttributes)
    {
        id ??= $"panel-{Guid.NewGuid()}";
        Id = id;
        State = state;
        EnsureHtmlAttribute("id", Id);
    }

    /// <inheritdoc/>
    protected internal override void StartTag(TextWriter textWriter) => Provider.PanelProvider.BeginPanel(this, textWriter);

    /// <inheritdoc/>
    protected internal override void EndTag(TextWriter textWriter) => Provider.PanelProvider.EndPanel(this, textWriter);
}