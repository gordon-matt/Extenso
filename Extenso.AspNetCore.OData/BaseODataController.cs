using Extenso.Data.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;

namespace Extenso.AspNetCore.OData;

/// <summary>
/// A generic, abstract CRUD controller for OData, with support for checking policy based permissions for users.
/// Get(TKey) in BaseODataController allows for OData query options, such as $expand, whereas GenericODataController does not
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey"></typeparam>
public abstract class BaseODataController<TEntity, TKey> : GenericODataController<TEntity, TKey>
    where TEntity : BaseEntity<TKey>
{
    protected BaseODataController(IAuthorizationService authorizationService, IRepository<TEntity> repository)
        : base(authorizationService, repository)
    {
    }

    /// <summary>
    /// Gets the record associated with the given key.
    /// </summary>
    /// <param name="key">The primary key value of the existing record.</param>
    /// <returns>The record associated with the given key.</returns>
    [EnableQuery]
    public override async Task<IActionResult> Get([FromODataUri] TKey key)
    {
        var connection = GetDisposableConnection();
        var query = connection.Query(x => x.Id.Equals(key));
        query = await ApplyMandatoryFilterAsync(query);
        var result = SingleResult.Create(query);

        var entity = result.Queryable.FirstOrDefault();
        return entity == null ? NotFound() : !await CanViewEntityAsync(entity) ? Unauthorized() : Ok(result);
    }

    protected override TKey GetId(TEntity entity) => entity.Id;
}