using Microsoft.EntityFrameworkCore;

namespace Extenso.Data.Entity;

public interface IEntityFrameworkRepository
{
    /// <summary>
    /// <para>Primarily used internally to get the DbContext for operations that require it.</para>
    /// <para>Only use this in cases where you need direct access to the DbContext, such as for raw SQL queries or advanced operations.</para>
    /// </summary>
    /// <returns>An instance of the DbContext.</returns>
    DbContext GetContext(ContextOptions options);
}