namespace Extenso.Data.Entity;

/// <summary>
/// The exception that is thrown for entity not found.
/// </summary>
[Serializable]
public class EntityNotFoundException : Exception
{
    /// <summary>
    /// Initializes a new instance of the EntityNotFoundException class.
    /// </summary>
    public EntityNotFoundException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the EntityNotFoundException class.
    /// </summary>
    /// <param name="message">The message.</param>
    public EntityNotFoundException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the EntityNotFoundException class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="innerException">The inner exception.</param>
    public EntityNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}