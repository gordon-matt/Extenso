using Extenso.Data.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;

namespace Extenso.AspNetCore.OData;

/// <summary>
/// A generic, abstract CRUD controller for OData, with support for checking policy based permissions for users.
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey"></typeparam>
public abstract class GenericODataController<TEntity, TKey> : ODataController, IDisposable
    where TEntity : class
{
    #region Non-Public Properties

    private IRepositoryConnection<TEntity> disposableConnection = null;

    /// <summary>
    /// Provides access to the data store.
    /// </summary>
    protected IRepository<TEntity> Repository { get; private set; }

    /// <summary>
    /// OData query options to ignore for querying.
    /// </summary>
    protected virtual AllowedQueryOptions IgnoreQueryOptions => AllowedQueryOptions.None;

    /// <summary>
    /// The name of the policy to evaluate for allowing users to read the data.
    /// </summary>
    protected virtual string ReadPermission => null;

    /// <summary>
    /// The name of the policy to evaluate for allowing users to save data.
    /// </summary>
    protected virtual string WritePermission => null;

    #endregion Non-Public Properties

    /// <summary>
    /// Checks policy based permissions for a user
    /// </summary>
    protected IAuthorizationService AuthorizationService { get; set; }

    #region Constructors

    /// <summary>
    /// Initializes a new instance of GenericODataController using the specified Extenso.Data.Entity.IRepository`1.
    /// </summary>
    /// <param name="repository">The Extenso.Data.Entity.IRepository`1 which will provide access to the data store.</param>
    public GenericODataController(IAuthorizationService authorizationService, IRepository<TEntity> repository)
    {
        AuthorizationService = authorizationService;
        Repository = repository;
    }

    #endregion Constructors

    #region Public Methods

    /// <summary>
    /// Applies the specified OData query options to an IQueryable to retrieve a collection of TEntity.
    /// </summary>
    /// <param name="options">The OData query options that can be used to perform query composition.</param>
    /// <returns>A collection of TEntity.</returns>
    [HttpGet]
    public virtual async Task<IActionResult> Get(ODataQueryOptions<TEntity> options, CancellationToken cancellationToken)
    {
        if (!await AuthorizeAsync(ReadPermission))
        {
            return Unauthorized();
        }

        // NOTE: Change due to: https://github.com/OData/WebApi/issues/1235
        var connection = GetDisposableConnection();
        var query = connection.Query();
        query = await ApplyMandatoryFilterAsync(query, cancellationToken);
        var results = options.ApplyTo(query, IgnoreQueryOptions);
        return Ok(results);
    }

    [HttpGet("stream")]
    [Route("~/odata/[controller]/stream")]
    public virtual async Task Stream(CancellationToken cancellationToken)
    {
        Response.ContentType = "application/x-ndjson";
        await foreach (var entity in Repository.StreamAsync(new SearchOptions<TEntity>(), cancellationToken))
        {
            await Response.WriteAsync(entity.JsonSerialize(), cancellationToken);
            await Response.WriteAsync("\n", cancellationToken);
            await Response.Body.FlushAsync(cancellationToken);
        }
    }

    // TODO: Can't do $expand, because entity is not from an IQueryable.. and also we don't have an ODataQueryOptions here..

    /// <summary>
    /// Gets the record associated with the given key.
    /// </summary>
    /// <param name="key">The primary key value of the existing record.</param>
    /// <returns>The record associated with the given key.</returns>
    [EnableQuery]
    [HttpGet]
    public virtual async Task<IActionResult> Get([FromODataUri] TKey key, CancellationToken cancellationToken)
    {
        var entity = await Repository.FindOneAsync(key);

        return entity is null
            ? NotFound() :
            !await CanViewEntityAsync(entity)
                ? Unauthorized()
                : Ok(entity);
    }

    /// <summary>
    /// Updates the record associated with the given key.
    /// </summary>
    /// <param name="key">The primary key value of the existing record.</param>
    /// <param name="entity">An instance of TEntity to use for updating the existing record.</param>
    /// <returns>A Microsoft.AspNet.OData.Results.UpdatedODataResult`1 with the specified values that is a response to the PUT operation.</returns>
    [HttpPut]
    public virtual async Task<IActionResult> Put([FromODataUri] TKey key, [FromBody] TEntity entity, CancellationToken cancellationToken)
    {
        if (entity is null)
        {
            return BadRequest();
        }

        if (!await CanModifyEntityAsync(entity))
        {
            return Unauthorized();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (!key.Equals(GetId(entity)))
        {
            return BadRequest();
        }

        try
        {
            OnBeforeSave(entity);
            await Repository.UpdateAsync(entity, ContextOptions.ForCancellationToken(cancellationToken));
            OnAfterSave(entity);
            return Updated(entity);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!EntityExists(key))
            {
                return NotFound();
            }
            else { throw; }
        }
    }

    /// <summary>
    /// Inserts a new record from the given TEntity.
    /// </summary>
    /// <param name="entity">The instance of TEntity to use for creating the new record.</param>
    /// <returns>A Microsoft.AspNet.OData.Results.CreatedODataResult`1 with the specified values that is a response to the POST operation.</returns>
    [HttpPost]
    public virtual async Task<IActionResult> Post([FromBody] TEntity entity, CancellationToken cancellationToken)
    {
        if (entity is null)
        {
            return BadRequest();
        }

        if (!await CanModifyEntityAsync(entity))
        {
            return Unauthorized();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        SetNewId(entity);

        OnBeforeSave(entity);
        await Repository.InsertAsync(entity, ContextOptions.ForCancellationToken(cancellationToken));
        OnAfterSave(entity);

        return Created(entity);
    }

    /// <summary>
    /// Updates the record associated with the given key.
    /// </summary>
    /// <param name="key">The primary key value of the existing record.</param>
    /// <param name="patch">The changes to apply to the existing record.</param>
    /// <returns>A Microsoft.AspNet.OData.Results.UpdatedODataResult`1 with the specified values that is a response to the PATCH or MERGE operation.</returns>
    [AcceptVerbs("PATCH", "MERGE")]
    public virtual async Task<IActionResult> Patch([FromODataUri] TKey key, Delta<TEntity> patch, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var entity = await Repository.FindOneAsync(key);

        if (entity is null)
        {
            return NotFound();
        }

        if (!await CanModifyEntityAsync(entity))
        {
            return Unauthorized();
        }

        patch.Patch(entity);

        try
        {
            await Repository.UpdateAsync(entity, ContextOptions.ForCancellationToken(cancellationToken));
            //db.SaveChanges();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!EntityExists(key))
            {
                return NotFound();
            }
            else { throw; }
        }

        return Updated(entity);
    }

    /// <summary>
    /// Deletes the record associated with the given key.
    /// </summary>
    /// <param name="key">The primary key value of the existing record.</param>
    /// <returns>A Microsoft.AspNetCore.Mvc.NoContentResult object that produces an empty response (HTTP Status 204).</returns>
    [HttpDelete]
    public virtual async Task<IActionResult> Delete([FromODataUri] TKey key, CancellationToken cancellationToken)
    {
        var entity = await Repository.FindOneAsync(key);

        if (entity is null)
        {
            return NotFound();
        }

        if (!await CanModifyEntityAsync(entity))
        {
            return Unauthorized();
        }

        await Repository.DeleteAsync(entity, ContextOptions.ForCancellationToken(cancellationToken));

        return NoContent();
    }

    #endregion Public Methods

    #region Non-Public Methods

    /// <summary>
    /// Gets a value indicating whether a record with the given primary key exists in the data store.
    /// </summary>
    /// <param name="key">The primary key value of the existing record.</param>
    /// <returns>true if a record with the given key exists; otherwise false.</returns>
    protected virtual bool EntityExists(TKey key) => Repository.FindOne(key) is not null;

    /// <summary>
    /// Gets the primary key value for the given record.
    /// </summary>
    /// <param name="entity">An instance of TEntity for which to obtain the primary key.</param>
    /// <returns>The primary key for the given record.</returns>
    protected abstract TKey GetId(TEntity entity);

    /// <summary>
    /// Creates a primary key value for the given entity. Generally, this should only be necessary if the key type is a System.Guid or a System.String.
    /// Tables with numeric keys usually don't need to have the primary key explicitly set.
    /// </summary>
    /// <param name="entity"></param>
    protected abstract void SetNewId(TEntity entity);

    /// <summary>
    /// Applies any filters which are mandatory for the current user. For example: if there is a "TenantId" field,
    /// you can filter by that so the user does not see data for the wrong site (tenant).
    /// </summary>
    /// <param name="query">The System.Linq.IQueryable`1 upon which to apply the filter.</param>
    /// <returns>A System.Linq.IQueryable`1 that may have had filters applied.</returns>
    protected virtual async Task<IQueryable<TEntity>> ApplyMandatoryFilterAsync(IQueryable<TEntity> query, CancellationToken cancellationToken) =>
        // Do nothing, by default
        await Task.FromResult(query);

    /// <summary>
    /// Gets a value indicating whether the given record can be viewed.
    /// </summary>
    /// <param name="entity">The record to test.</param>
    /// <returns>true if the record can be viewed; otherwise false.</returns>
    protected virtual async Task<bool> CanViewEntityAsync(TEntity entity) => await AuthorizeAsync(ReadPermission);

    /// <summary>
    /// Gets a value indicating whether the given record can be modified.
    /// </summary>
    /// <param name="entity">The record to test.</param>
    /// <returns>true if the record can be modified; otherwise false.</returns>
    protected virtual async Task<bool> CanModifyEntityAsync(TEntity entity) => await AuthorizeAsync(WritePermission);

    /// <summary>
    /// Perform actions before inserting or updating the record. For example, you may wish to update a timestamp field on the given record.
    /// </summary>
    /// <param name="entity">The record which is about to be inserted or updated.</param>
    protected virtual void OnBeforeSave(TEntity entity)
    {
    }

    /// <summary>
    /// Perform actions after inserting or updating the record. For example, you may wish to push some kind of notification.
    /// </summary>
    /// <param name="entity">The record which has been inserted or updated.</param>
    protected virtual void OnAfterSave(TEntity entity)
    {
    }

    /// <summary>
    /// Checks if the current user meets a specific authorization policy against the specified resource.
    /// </summary>
    /// <param name="policyName">The name of the policy to evaluate.</param>
    /// <returns>
    /// A flag indicating whether policy evaluation has succeeded or failed. This value is
    /// true when the user fulfills the policy; otherwise false.
    /// </returns>
    protected virtual async Task<bool> AuthorizeAsync(string policyName)
    {
        if (string.IsNullOrEmpty(policyName))
        {
            return true;
        }

        if (AuthorizationService is null)
        {
            return false;
        }

        var result = await AuthorizationService.AuthorizeAsync(User, policyName);
        return result.Succeeded;
    }

    #endregion Non-Public Methods

    /// <summary>
    /// Opens a connection to the data store. Remember to dispose of it when no longer needed.
    /// </summary>
    /// <returns>An Extenso.Data.Entity.IRepositoryConnection`1 that provides access to the data store.</returns>
    protected IRepositoryConnection<TEntity> GetDisposableConnection() => disposableConnection ??= Repository.OpenConnection();

    #region IDisposable Support

    private bool disposedValue = false; // To detect redundant calls

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                disposableConnection?.Dispose();
            }

            // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
            // TODO: set large fields to null.

            disposedValue = true;
        }
    }

    // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
    // ~GenericODataController() {
    //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
    //   Dispose(false);
    // }

    // This code added to correctly implement the disposable pattern.
    public void Dispose() =>
        // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        Dispose(true);// TODO: uncomment the following line if the finalizer is overridden above.// GC.SuppressFinalize(this);

    #endregion IDisposable Support
}