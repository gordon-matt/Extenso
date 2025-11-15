namespace Extenso;

/// <summary>
/// Provides a set of static methods for querying and manipulating instances of System.Guid.
/// </summary>
public static class GuidExtensions
{
    extension(Guid source)
    {
        /// <summary>
        /// Gets a value indicating whether the specified System.Guid is empty
        /// </summary>
        /// <returns>true if value is empty (00000000-0000-0000-0000-000000000000); otherwise, false.</returns>
        public bool IsEmpty() => source == Guid.Empty;
    }

    extension(Guid? source)
    {
        /// <summary>
        /// Gets a value indicating whether the specified System.Guid is null or empty
        /// </summary>
        /// <returns>true if value is null or empty (00000000-0000-0000-0000-000000000000); otherwise, false.</returns>
        public bool IsNullOrEmpty() => source is null || source == Guid.Empty;
    }
}