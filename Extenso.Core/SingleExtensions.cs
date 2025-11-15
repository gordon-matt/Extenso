namespace Extenso;

/// <summary>
/// Provides a set of static methods for querying and manipulating instances of System.Single.
/// </summary>
public static class SingleExtensions
{
    extension(float? source)
    {
        /// <summary>
        /// Gets a value indicating whether the given nullable System.Single is null or the default.
        /// </summary>
        /// <returns>true if source is null or the default; otherwise, false.</returns>
        public bool IsNullOrDefault() => source is null or default(float);
    }
}