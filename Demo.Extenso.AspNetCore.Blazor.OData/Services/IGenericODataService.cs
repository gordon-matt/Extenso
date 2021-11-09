using System;
using System.Net.Http;
using System.Threading.Tasks;
using Radzen;

namespace Demo.Extenso.AspNetCore.Blazor.OData.Services
{
    public interface IGenericODataService<TEntity, TKey> : IDisposable
        where TEntity : class
    {
        Task<ODataServiceResult<TEntity>> FindAsync(
            string filter = default,
            int? top = default,
            int? skip = default,
            string orderby = default,
            string expand = default,
            string select = default,
            bool? count = default);

        Task<TEntity> FindOneAsync(TKey key);

        Task<TEntity> InsertAsync(TEntity entity);

        Task<TEntity> UpdateAsync(TKey key, TEntity entity);

        Task<HttpResponseMessage> DeleteAsync(TKey key);
    }
}