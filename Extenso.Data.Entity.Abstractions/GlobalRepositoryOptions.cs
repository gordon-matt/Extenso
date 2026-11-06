namespace Extenso.Data.Entity;

/// <summary>
/// Provides global repository options.
/// </summary>
public static class GlobalRepositoryOptions
{
    /// <summary>
    /// Gets or sets a value indicating whether this instance is tag with call site.
    /// </summary>
    public static bool TagWithCallSite { get; set; } = true;
}