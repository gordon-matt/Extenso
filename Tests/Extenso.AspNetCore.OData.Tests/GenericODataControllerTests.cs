using Bogus;
using Extenso.Data.Entity;
using Extenso.TestLib.Data;
using Extenso.TestLib.Data.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Tests.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.OData.ModelBuilder;
using Moq;

namespace Extenso.AspNetCore.OData.Tests;

public class GenericODataControllerTests : IDisposable
{
    private readonly InMemoryAdventureWorks2019ContextFactory contextFactory;
    private readonly IRepository<ProductModel> repository;
    private readonly ProductModelApiController odataController;
    private readonly ICollection<ProductModel> productModels;
    private readonly Faker<ProductModel> productModelFaker;
    private readonly IAuthorizationService authorizationService;

    private bool isDisposed;

    public GenericODataControllerTests()
    {
        var optionsBuilder = new DbContextOptionsBuilder<AdventureWorks2019Context>();
        optionsBuilder.UseInMemoryDatabase("AdventureWorks2019");
        using var context = new AdventureWorks2019Context(optionsBuilder.Options);

        productModelFaker = new Faker<ProductModel>()
            .RuleFor(x => x.Name, x => x.Commerce.Department())
            .RuleFor(x => x.CatalogDescription, x => x.Commerce.ProductDescription())
            .RuleFor(x => x.Instructions, x => x.Lorem.Paragraph())
            .RuleFor(x => x.Rowguid, x => x.Random.Guid())
            .RuleFor(x => x.ModifiedDate, x => x.Date.Between(DateTime.Today.AddYears(-10), DateTime.Today.AddDays(-1)));

        productModels = productModelFaker.Generate(100);
        context.ProductModels.AddRange(productModels);
        context.SaveChanges();

        authorizationService = new FakeAuthorizationService(AuthorizationState.Authorized);

        contextFactory = new InMemoryAdventureWorks2019ContextFactory();
        repository = new EntityFrameworkRepository<ProductModel>(contextFactory, Mock.Of<ILoggerFactory>());
        odataController = new ProductModelApiController(authorizationService, repository);
    }

    [Fact]
    public async Task GetAll_Returns_All()
    {
        var actionResult = await odataController.Get(MockODataQueryOptions<ProductModel>());
        actionResult.Should().BeOfType<OkObjectResult>();

        var okObjectResult = (OkObjectResult)actionResult;
        okObjectResult.Value.Should().NotBeNull();
        okObjectResult.Value.Should().BeAssignableTo(typeof(IQueryable));

        var query = (IQueryable<ProductModel>)okObjectResult.Value;
        var results = query.ToList();

        results.Should().BeEquivalentTo(productModels);
    }

    [Fact]
    public async Task GetOne_Returns_Entity()
    {
        var randomProductModel = new Random().NextFrom(productModels);
        int id = randomProductModel.ProductModelId;

        var actionResult = await odataController.Get(id);
        actionResult.Should().BeOfType<OkObjectResult>();

        var okObjectResult = (OkObjectResult)actionResult;
        okObjectResult.Value.Should().NotBeNull();
        okObjectResult.Value.Should().BeOfType(typeof(SingleResult<ProductModel>));

        var result = (SingleResult<ProductModel>)okObjectResult.Value;
        var entity = result.Queryable.Single();
        entity.Should().NotBeNull();
        entity.Should().BeEquivalentTo(randomProductModel);
    }

    [Fact]
    public async Task GetOne_Returns_NotFound()
    {
        int id = productModels.Max(x => x.ProductModelId) + 1;

        var actionResult = await odataController.Get(id);
        actionResult.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task Post_Returns_Created()
    {
        var entity = productModelFaker.Generate();
        var actionResult = await odataController.Post(entity);
        actionResult.Should().BeOfType<CreatedODataResult<ProductModel>>();

        var createdResult = (CreatedODataResult<ProductModel>)actionResult;
        createdResult.Entity.Should().NotBeNull();
        createdResult.Entity.ProductModelId.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task Post_Returns_BadRequest()
    {
        var actionResult = await odataController.Post(null);
        actionResult.Should().BeOfType<BadRequestResult>();
    }

    [Fact]
    public async Task Put_Returns_Updated()
    {
        var entities = repository.Find();
        var entity = new Random().NextFrom(entities);
        entity.Name = "Foo Bar Baz";

        var actionResult = await odataController.Put(entity.ProductModelId, entity);
        actionResult.Should().BeOfType<UpdatedODataResult<ProductModel>>();

        var updatedResult = (UpdatedODataResult<ProductModel>)actionResult;
        updatedResult.Entity.Should().NotBeNull();
        updatedResult.Entity.Name.Should().Be("Foo Bar Baz");
    }

    [Fact]
    public async Task Put_Returns_BadRequest()
    {
        var actionResult = await odataController.Put(1, null);
        actionResult.Should().BeOfType<BadRequestResult>();
    }

    [Fact]
    public async Task Put_Returns_BadRequest_2()
    {
        var entities = repository.Find();
        var entity = new Random().NextFrom(entities);
        var actionResult = await odataController.Put(entity.ProductModelId + 1, entity);
        actionResult.Should().BeOfType<BadRequestResult>();
    }

    [Fact]
    public async Task Patch_Returns_Updated()
    {
        var entities = repository.Find();
        var entity = new Random().NextFrom(entities);
        string catalogDescription = entity.CatalogDescription; // Keeping track of this to ensure the patch doesn't override anything other than what we're setting (Name)

        var patch = new Delta<ProductModel>();
        patch.TrySetPropertyValue("Name", "Foo Bar Baz");

        var actionResult = await odataController.Patch(entity.ProductModelId, patch);
        actionResult.Should().BeOfType<UpdatedODataResult<ProductModel>>();

        var updatedResult = (UpdatedODataResult<ProductModel>)actionResult;
        updatedResult.Entity.Should().NotBeNull();
        updatedResult.Entity.Name.Should().Be("Foo Bar Baz");
        updatedResult.Entity.CatalogDescription.Should().Be(catalogDescription); // Should not have changed (to NULL or whatever)
    }

    [Fact]
    public async Task Patch_Returns_NotFound()
    {
        int id = productModels.Max(x => x.ProductModelId) + 1;
        var actionResult = await odataController.Patch(id, null);
        actionResult.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task Delete_Returns_NoContent()
    {
        int id = productModels.Max(x => x.ProductModelId);
        var actionResult = await odataController.Delete(id);
        actionResult.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task Delete_Returns_NotFound()
    {
        int id = productModels.Max(x => x.ProductModelId) + 1;
        var actionResult = await odataController.Delete(id);
        actionResult.Should().BeOfType<NotFoundResult>();
    }

    private static ODataQueryOptions<T> MockODataQueryOptions<T>()
    {
        var builder = new ODataConventionModelBuilder();
        builder.EntitySet<ProductModel>($"{typeof(T).Name}Api");
        var edmModel = builder.GetEdmModel();

        var request = RequestFactory.Create(HttpMethods.Get, "http://any");
        var queryContext = new ODataQueryContext(edmModel, typeof(T), path: null);
        return new ODataQueryOptions<T>(queryContext, request);
    }

    #region Dispose Pattern

    protected virtual void Dispose(bool disposing)
    {
        if (!isDisposed)
        {
            if (disposing)
            {
                using var context = contextFactory.GetContext();
                context.Database.EnsureDeleted(); // Necessary to reset EF in-memory provider between tests..

                odataController?.Dispose();
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            isDisposed = true;
        }
    }

    // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    // ~EntityFrameworkRepositoryTests()
    // {
    //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
    //     Dispose(disposing: false);
    // }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    #endregion Dispose Pattern
}