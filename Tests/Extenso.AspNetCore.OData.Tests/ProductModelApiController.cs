using Extenso.Data.Entity;
using Extenso.TestLib.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;

namespace Extenso.AspNetCore.OData.Tests;

public class ProductModelApiController : GenericODataController<ProductModel, int>
{
    public ProductModelApiController(IAuthorizationService authorizationService, IRepository<ProductModel> repository)
        : base(authorizationService, repository)
    {
    }

    [EnableQuery]
    public override async Task<IActionResult> Get([FromODataUri] int key)
    {
        var connection = GetDisposableConnection();
        var query = connection.Query(x => x.ProductModelId.Equals(key));
        query = await ApplyMandatoryFilterAsync(query);
        var result = SingleResult.Create(query);

        var entity = result.Queryable.FirstOrDefault();
        return entity == null ? NotFound() : !await CanViewEntityAsync(entity) ? Unauthorized() : Ok(result);
    }

    protected override int GetId(ProductModel entity) => entity.ProductModelId;

    protected override void SetNewId(ProductModel entity)
    {
    }
}