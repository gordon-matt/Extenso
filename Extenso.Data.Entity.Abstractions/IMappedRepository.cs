using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Extenso.Data.Entity;

public interface IMappedRepository<TModel, TEntity> : IRepository<TModel>
    where TModel : class
    where TEntity : class, IEntity
{
    TModel ToModel(TEntity entity);

    TEntity ToEntity(TModel model);
}