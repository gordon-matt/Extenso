using Demo.Extenso.AspNetCore.Blazor.OData.Models;
using Radzen;

namespace Demo.Extenso.AspNetCore.Blazor.OData.Services;

public interface IGenericODataService<TEntity, TKey> : IDisposable
    where TEntity : class
{
    Task<ApiResponse<ODataServiceResult<TEntity>>> FindAsync(
        string filter = default,
        int? top = default,
        int? skip = default,
        string orderby = default,
        string expand = default,
        string select = default,
        bool? count = default,
        CancellationToken cancellationToken = default);

    Task<ApiResponse<TEntity>> FindOneAsync(TKey key, CancellationToken cancellationToken = default);

    Task<ApiResponse<TEntity>> InsertAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task<ApiResponse<TEntity>> UpdateAsync(TKey key, TEntity entity, CancellationToken cancellationToken = default);

    Task<ApiResponse> DeleteAsync(TKey key, CancellationToken cancellationToken = default);
}