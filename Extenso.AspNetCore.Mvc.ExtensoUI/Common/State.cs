namespace Extenso.AspNetCore.Mvc.ExtensoUI;

/// <summary>
/// Specifies the state.
/// </summary>
public enum State : byte
{
    /// <summary>
    /// The default value.
    /// </summary>
    Default,
    /// <summary>
    /// The primary value.
    /// </summary>
    Primary,
    /// <summary>
    /// The info value.
    /// </summary>
    Info,
    /// <summary>
    /// The success value.
    /// </summary>
    Success,
    /// <summary>
    /// The warning value.
    /// </summary>
    Warning,
    /// <summary>
    /// The danger value.
    /// </summary>
    Danger,
    /// <summary>
    /// The inverse value.
    /// </summary>
    Inverse
}