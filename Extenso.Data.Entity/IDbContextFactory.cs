using Microsoft.EntityFrameworkCore;

namespace Extenso.Data.Entity;

/// <summary>
/// Defines the contract for the database context factory.
/// </summary>
public interface IDbContextFactory
{
    /// <summary>
    /// Gets the context.
    /// </summary>
    /// <returns>The context.</returns>
    DbContext GetContext();

    /// <summary>
    /// Gets the context.
    /// </summary>
    /// <param name="connectionString">The connection string.</param>
    /// <returns>The context.</returns>
    DbContext GetContext(string connectionString);
}