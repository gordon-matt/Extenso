using Microsoft.EntityFrameworkCore;

namespace Extenso.Data.Entity;

public interface IEntityFrameworkRepositoryConnection<TEntity> : IRepositoryConnection<TEntity>
{
    DbContext Context { get; }
}