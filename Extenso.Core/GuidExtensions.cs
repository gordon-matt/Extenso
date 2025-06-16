namespace Extenso;

/// <summary>
/// Provides a set of static methods for querying and manipulating instances of System.Guid.
/// </summary>
public static class GuidExtensions
{
    /// <summary>
    /// Gets a value indicating whether the specified System.Guid is empty
    /// </summary>
    /// <param name="value">The System.Guid to examine.</param>
    /// <returns>true if value is empty (00000000-0000-0000-0000-000000000000); otherwise, false.</returns>
    public static bool IsEmpty(this Guid value) => value == Guid.Empty;

    /// <summary>
    /// Gets a value indicating whether the specified System.Guid is null or empty
    /// </summary>
    /// <param name="value">The System.Guid to examine.</param>
    /// <returns>true if value is null or empty (00000000-0000-0000-0000-000000000000); otherwise, false.</returns>
    public static bool IsNullOrEmpty(this Guid? value) => value is null || value == Guid.Empty;
}