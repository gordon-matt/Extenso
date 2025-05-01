namespace Extenso.Data.Entity;

public interface IMappedRepository<TModel, TEntity> : IRepository<TModel>
    where TModel : class
    where TEntity : class, IEntity
{
}