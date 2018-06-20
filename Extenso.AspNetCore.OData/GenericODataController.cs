using System;
using System.Linq;
using System.Threading.Tasks;
using Extenso.Collections;
using Extenso.Data.Entity;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Extenso.Web.OData
{
    public abstract class GenericODataController<TEntity, TKey> : ODataController, IDisposable
        where TEntity : class
    {
        #region Non-Public Properties

        private IRepositoryConnection<TEntity> disposableConnection = null;

        protected IRepository<TEntity> Repository { get; private set; }

        protected virtual AllowedQueryOptions IgnoreQueryOptions => AllowedQueryOptions.None;

        protected virtual string ReadPermission => null;

        protected virtual string WritePermission => null;

        #endregion Non-Public Properties

        public IAuthorizationService AuthorizationService { get; set; }

        #region Constructors

        public GenericODataController(IRepository<TEntity> repository)
        {
            Repository = repository;
        }

        #endregion Constructors

        #region Public Methods

        // GET: odata/<Entity>
        public virtual async Task<IActionResult> Get(ODataQueryOptions<TEntity> options)
        {
            if (!await AuthorizeAsync(ReadPermission))
            {
                return Unauthorized();
            }

            //    using (var connection = Service.OpenConnection())
            //    {
            //        var query = connection.Query();
            //        query = ApplyMandatoryFilter(query);
            //        var results = options.ApplyTo(query);
            //        return await (results as IQueryable<TEntity>).ToHashSetAsync();
            //    }

            // NOTE: Change due to: https://github.com/OData/WebApi/issues/1235
            var connection = GetDisposableConnection();
            var query = connection.Query();
            query = ApplyMandatoryFilter(query);
            var results = options.ApplyTo(query, IgnoreQueryOptions);

            // Recommended not to use ToHashSetAsync(). See: https://github.com/OData/WebApi/issues/1235#issuecomment-371322404
            //return await (results as IQueryable<TEntity>).ToHashSetAsync();

            var response = await Task.FromResult((results as IQueryable<TEntity>).ToHashSet());
            return Ok(response);
        }

        // GET: odata/<Entity>(5)
        [EnableQuery]
        public virtual async Task<IActionResult> Get([FromODataUri] TKey key)
        {
            var entity = await Repository.FindOneAsync(key);

            if (entity == null)
            {
                return NotFound();
            }

            if (!await CanViewEntity(entity))
            {
                return Unauthorized();
            }

            return Ok(entity);
        }

        // PUT: odata/<Entity>(5)
        public virtual async Task<IActionResult> Put([FromODataUri] TKey key, [FromBody] TEntity entity)
        {
            if (entity == null)
            {
                return BadRequest();
            }

            if (!await CanModifyEntity(entity))
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
                await Repository.UpdateAsync(entity);
                OnAfterSave(entity);
            }
            catch (DbUpdateConcurrencyException x)
            {
                if (!EntityExists(key))
                {
                    return NotFound();
                }
                else { throw; }
            }

            return Updated(entity);
        }

        // POST: odata/<Entity>
        public virtual async Task<IActionResult> Post([FromBody] TEntity entity)
        {
            if (entity == null)
            {
                return BadRequest();
            }

            if (!await CanModifyEntity(entity))
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            SetNewId(entity);

            OnBeforeSave(entity);
            await Repository.InsertAsync(entity);
            OnAfterSave(entity);

            return Created(entity);
        }

        // PATCH: odata/<Entity>(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public virtual async Task<IActionResult> Patch([FromODataUri] TKey key, Delta<TEntity> patch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            TEntity entity = await Repository.FindOneAsync(key);

            if (entity == null)
            {
                return NotFound();
            }

            if (!await CanModifyEntity(entity))
            {
                return Unauthorized();
            }

            patch.Patch(entity);

            try
            {
                await Repository.UpdateAsync(entity);
                //db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException x)
            {
                if (!EntityExists(key))
                {
                    return NotFound();
                }
                else { throw; }
            }

            return Updated(entity);
        }

        // DELETE: odata/<Entity>(5)
        public virtual async Task<IActionResult> Delete([FromODataUri] TKey key)
        {
            TEntity entity = await Repository.FindOneAsync(key);

            if (entity == null)
            {
                return NotFound();
            }

            if (!await CanModifyEntity(entity))
            {
                return Unauthorized();
            }

            await Repository.DeleteAsync(entity);

            return NoContent();
        }

        #endregion Public Methods

        #region Non-Public Methods

        protected virtual bool EntityExists(TKey key)
        {
            return Repository.FindOne(key) != null;
        }

        protected abstract TKey GetId(TEntity entity);

        /// <summary>
        /// Should only be necessary for Guid types
        /// </summary>
        /// <param name="entity"></param>
        protected abstract void SetNewId(TEntity entity);

        /// <summary>
        /// Add any filters which must be applied for the client. Mostly used for fields such as "TenantId", where you don't want
        /// the user viewing data for a different site (tenant)
        /// </summary>
        /// <param name="entity"></param>
        protected virtual IQueryable<TEntity> ApplyMandatoryFilter(IQueryable<TEntity> query)
        {
            // Do nothing, by default
            return query;
        }

        protected virtual async Task<bool> CanViewEntity(TEntity entity)
        {
            return await AuthorizeAsync(ReadPermission);
        }

        protected virtual async Task<bool> CanModifyEntity(TEntity entity)
        {
            return await AuthorizeAsync(WritePermission);
        }

        protected virtual void OnBeforeSave(TEntity entity)
        {
        }

        protected virtual void OnAfterSave(TEntity entity)
        {
        }

        protected virtual async Task<bool> AuthorizeAsync(string policyName)
        {
            if (AuthorizationService == null || string.IsNullOrEmpty(policyName))
            {
                return true;
            }

            var result = await AuthorizationService.AuthorizeAsync(User, policyName);
            return result.Succeeded;
        }

        #endregion Non-Public Methods

        protected IRepositoryConnection<TEntity> GetDisposableConnection()
        {
            if (disposableConnection == null)
            {
                disposableConnection = Repository.OpenConnection();
            }
            return disposableConnection;
        }

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    if (disposableConnection != null)
                    {
                        disposableConnection.Dispose();
                        disposableConnection = null;
                    }
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
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        #endregion IDisposable Support
    }
}