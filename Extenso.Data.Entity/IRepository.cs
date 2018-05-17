using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Extenso.Data.Entity
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IRepositoryConnection<TEntity> OpenConnection();

        IRepositoryConnection<TEntity> UseConnection<TOther>(IRepositoryConnection<TOther> connection)
            where TOther : class;

        #region Find

        IEnumerable<TEntity> Find(params Expression<Func<TEntity, dynamic>>[] includePaths);

        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, dynamic>>[] includePaths);

        Task<IEnumerable<TEntity>> FindAsync(params Expression<Func<TEntity, dynamic>>[] includePaths);

        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, dynamic>>[] includePaths);

        TEntity FindOne(params object[] keyValues);

        TEntity FindOne(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, dynamic>>[] includePaths);

        Task<TEntity> FindOneAsync(params object[] keyValues);

        Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, dynamic>>[] includePaths);

        #endregion Find

        #region Count

        int Count();

        int Count(Expression<Func<TEntity, bool>> predicate);

        Task<int> CountAsync();

        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);

        #endregion Count

        #region Delete

        int DeleteAll();

        int Delete(TEntity entity);

        int Delete(IEnumerable<TEntity> entities);

        int Delete(Expression<Func<TEntity, bool>> predicate);

        Task<int> DeleteAllAsync();

        Task<int> DeleteAsync(TEntity entity);

        Task<int> DeleteAsync(IEnumerable<TEntity> entities);

        Task<int> DeleteAsync(Expression<Func<TEntity, bool>> predicate);

        #endregion Delete

        #region Insert

        int Insert(TEntity entity);

        int Insert(IEnumerable<TEntity> entities);

        Task<int> InsertAsync(TEntity entity);

        Task<int> InsertAsync(IEnumerable<TEntity> entities);

        #endregion Insert

        #region Update

        int Update(TEntity entity);

        int Update(IEnumerable<TEntity> entities);

        Task<int> UpdateAsync(TEntity entity);

        Task<int> UpdateAsync(IEnumerable<TEntity> entities);

        #endregion Update
    }
}