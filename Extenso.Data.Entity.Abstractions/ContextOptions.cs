using System.Data.Common;

namespace Extenso.Data.Entity;

/// <summary>
/// Represents the context options.
/// </summary>
public class ContextOptions
{
    /// <summary>
    /// Gets or sets the command timeout.
    /// </summary>
    public int? CommandTimeout { get; set; }

    /// <summary>
    /// Gets or sets the transaction.
    /// </summary>
    public DbTransaction Transaction { get; set; }

    /// <summary>
    /// Gets or sets the cancellation token.
    /// </summary>
    public CancellationToken CancellationToken { get; set; } = default;

    /// <summary>
    /// Gets the for cancellation token.
    /// </summary>
    /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
    /// <returns>The for cancellation token.</returns>
    public static ContextOptions ForCancellationToken(CancellationToken cancellationToken) => new() { CancellationToken = cancellationToken };
}