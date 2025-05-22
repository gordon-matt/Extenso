using System.Drawing;
using Bogus;
using Extenso.TestLib.Data;
using Extenso.TestLib.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Z.EntityFramework.Plus;

namespace Extenso.Data.Entity.Tests;

[Collection("EntityFrameworkRepositoryTests")]
public class EntityFrameworkRepositoryTests : IDisposable
{
    private readonly InMemoryAdventureWorks2019ContextFactory contextFactory;
    private readonly IRepository<ProductModel> repository;
    private readonly ICollection<ProductModel> productModels;
    private readonly ICollection<Product> products;
    private bool isDisposed;

    private readonly Faker<ProductModel> productModelFaker;
    private readonly Faker<Product> productFaker;

    public EntityFrameworkRepositoryTests()
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

        productFaker = new Faker<Product>()
            .RuleFor(x => x.ProductModelId, x => x.PickRandom(productModels).ProductModelId)
            .RuleFor(x => x.Name, x => x.Commerce.ProductName())
            .RuleFor(x => x.ProductNumber, x => x.Commerce.Ean13())
            .RuleFor(x => x.Color, x => x.PickRandom(EnumExtensions.GetValues<KnownColor>()).ToString())
            .RuleFor(x => x.FinishedGoodsFlag, x => x.Random.Bool())
            .RuleFor(x => x.MakeFlag, x => x.Random.Bool())
            .RuleFor(x => x.Rowguid, x => x.Random.Guid())
            .RuleFor(x => x.ModifiedDate, x => x.Date.Between(DateTime.Today.AddYears(-10), DateTime.Today.AddDays(-1)));

        products = productFaker.Generate(100);
        context.Products.AddRange(products);
        context.SaveChanges();

        contextFactory = new InMemoryAdventureWorks2019ContextFactory();
        repository = new EntityFrameworkRepository<ProductModel>(contextFactory, Mock.Of<ILoggerFactory>());
    }

    #region Find

    [Fact]
    public void Find_IncludePaths()
    {
        var randomProduct = new Random().NextFrom(products);

        int expected = products.Count(x => x.ProductModelId == randomProduct.ProductModelId);

        int actual = repository
            .Find(new SearchOptions<ProductModel>
            {
                Include = query => query.Include(x => x.Products)
            })
            .Where(x => x.ProductModelId == randomProduct.ProductModelId)
            .SelectMany(x => x.Products)
            .Count();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Find_Predicate_And_IncludePaths()
    {
        var randomProduct = new Random().NextFrom(products);
        var productModel = productModels.First(x => x.ProductModelId == randomProduct.ProductModelId);
        char firstLetter = productModel.Name[0];

        int expected = products.Count(x => x.ProductModelId == randomProduct.ProductModelId);

        int actual = repository
            .Find(new SearchOptions<ProductModel>
            {
                Query = x => x.Name.StartsWith(firstLetter),
                Include = query => query.Include(x => x.Products)
            })
            .Where(x => x.ProductModelId == randomProduct.ProductModelId)
            .SelectMany(x => x.Products)
            .Count();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task FindAsync_IncludePaths()
    {
        var randomProduct = new Random().NextFrom(products);

        int expected = products.Count(x => x.ProductModelId == randomProduct.ProductModelId);

        var query = await repository.FindAsync(new SearchOptions<ProductModel>
        {
            Include = query => query.Include(x => x.Products)
        });

        int actual = query.Where(x => x.ProductModelId == randomProduct.ProductModelId)
            .SelectMany(x => x.Products)
            .Count();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task FindAsync_Predicate_And_IncludePaths()
    {
        var randomProduct = new Random().NextFrom(products);
        var productModel = productModels.First(x => x.ProductModelId == randomProduct.ProductModelId);
        char firstLetter = productModel.Name[0];

        int expected = products.Count(x => x.ProductModelId == randomProduct.ProductModelId);

        var query = await repository.FindAsync(new SearchOptions<ProductModel>
        {
            Query = x => x.Name.StartsWith(firstLetter),
            Include = query => query.Include(x => x.Products)
        });

        int actual = query.Where(x => x.ProductModelId == randomProduct.ProductModelId)
            .SelectMany(x => x.Products)
            .Count();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void FindOne()
    {
        var randomProduct = new Random().NextFrom(products);
        var entity = repository.FindOne(randomProduct.ProductModelId);
        Assert.NotNull(entity);
    }

    [Fact]
    public void FindOne_Predicate_And_IncludePaths()
    {
        var randomProduct = new Random().NextFrom(products);
        var productModel = productModels.First(x => x.ProductModelId == randomProduct.ProductModelId);
        var entity = repository.FindOne(new SearchOptions<ProductModel>
        {
            Query = x => x.Name == productModel.Name,
            Include = query => query.Include(x => x.Products)
        });
        Assert.NotNull(entity);
    }

    [Fact]
    public async Task FindOneAsync()
    {
        var randomProduct = new Random().NextFrom(products);
        var entity = await repository.FindOneAsync(randomProduct.ProductModelId);
        Assert.NotNull(entity);
    }

    [Fact]
    public async Task FindOneAsync_Predicate_And_IncludePaths()
    {
        var randomProduct = new Random().NextFrom(products);
        var productModel = productModels.First(x => x.ProductModelId == randomProduct.ProductModelId);
        var entity = await repository.FindOneAsync(new SearchOptions<ProductModel>
        {
            Query = x => x.Name == productModel.Name,
            Include = query => query.Include(x => x.Products)
        });
        Assert.NotNull(entity);
    }

    #endregion Find

    #region Count

    [Fact]
    public void Count()
    {
        int expected = productModels.Count;
        int actual = repository.Count();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Count_With_Predicate()
    {
        int expected = productModels.Count(x => x.Name.StartsWith("M"));
        int actual = repository.Count(x => x.Name.StartsWith("M"));
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task CountAsync()
    {
        int expected = productModels.Count;
        int actual = await repository.CountAsync();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task CountAsync_With_Predicate()
    {
        int expected = productModels.Count(x => x.Name.StartsWith("M"));
        int actual = await repository.CountAsync(x => x.Name.StartsWith("M"));
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void LongCount()
    {
        long expected = productModels.LongCount();
        long actual = repository.LongCount();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void LongCount_With_Predicate()
    {
        long expected = productModels.LongCount(x => x.Name.StartsWith("M"));
        long actual = repository.LongCount(x => x.Name.StartsWith("M"));
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task LongCountAsync()
    {
        long expected = productModels.LongCount();
        long actual = await repository.LongCountAsync();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task LongCountAsync_With_Predicate()
    {
        long expected = productModels.LongCount(x => x.Name.StartsWith("M"));
        long actual = await repository.LongCountAsync(x => x.Name.StartsWith("M"));
        Assert.Equal(expected, actual);
    }

    #endregion Count

    #region Delete

    // repository.DeleteAll() relies on Z.EntityFramework.Plus which does not seem to support in memory db.
    //[Fact]
    //public void DeleteAll();

    [Fact]
    public void Delete()
    {
        int count = repository.Count();

        var randomProductModel = new Random().NextFrom(productModels);
        var entity = repository.FindOne(randomProductModel.ProductModelId);
        Assert.NotNull(entity);

        int rowsAffected = repository.Delete(entity);
        Assert.Equal(1, rowsAffected);

        int newCount = repository.Count();
        Assert.Equal(count - 1, newCount);
    }

    [Fact]
    public void DeleteMany()
    {
        int count = repository.Count();

        var entities = repository.Find(new SearchOptions<ProductModel>
        {
            Query = x => true
        }).Take(5);
        Assert.NotEmpty(entities);

        int rowsAffected = repository.Delete(entities);
        Assert.Equal(5, rowsAffected);

        int newCount = repository.Count();
        Assert.Equal(count - 5, newCount);
    }

    // repository.DeleteWhere() relies on Z.EntityFramework.Plus which does not seem to support in memory db.
    //[Fact]
    //public void DeleteWhere();

    // repository.DeleteAllAsync() relies on Z.EntityFramework.Plus which does not seem to support in memory db.
    //[Fact]
    //public async Task DeleteAllAsync();

    [Fact]
    public async Task DeleteAsync()
    {
        int count = await repository.CountAsync();

        var randomProductModel = new Random().NextFrom(productModels);
        var entity = await repository.FindOneAsync(randomProductModel.ProductModelId);
        Assert.NotNull(entity);

        int rowsAffected = await repository.DeleteAsync(entity);
        Assert.Equal(1, rowsAffected);

        int newCount = await repository.CountAsync();
        Assert.Equal(count - 1, newCount);
    }

    [Fact]
    public async Task DeleteManyAsync()
    {
        int count = await repository.CountAsync();

        var entities = (await repository.FindAsync(new SearchOptions<ProductModel>
        {
            Query = x => true
        })).Take(5);
        Assert.NotEmpty(entities);

        int rowsAffected = await repository.DeleteAsync(entities);
        Assert.Equal(5, rowsAffected);

        int newCount = await repository.CountAsync();
        Assert.Equal(count - 5, newCount);
    }

    // repository.DeleteWhereAsync() relies on Z.EntityFramework.Plus which does not seem to support in memory db.
    //[Fact]
    //public async Task DeleteWhereAsync();

    #endregion Delete

    #region Insert

    [Fact]
    public void Insert()
    {
        int count = repository.Count();
        var newProductModel = productModelFaker.Generate();

        int rowsAffected = repository.Insert(newProductModel);
        Assert.Equal(1, rowsAffected);

        int newCount = repository.Count();
        Assert.Equal(count + 1, newCount);
    }

    [Fact]
    public void InsertMany()
    {
        int count = repository.Count();
        var newProductModels = productModelFaker.Generate(10);

        int rowsAffected = repository.Insert(newProductModels);
        Assert.Equal(10, rowsAffected);

        int newCount = repository.Count();
        Assert.Equal(count + 10, newCount);
    }

    [Fact]
    public async Task InsertAsync()
    {
        int count = await repository.CountAsync();
        var newProductModel = productModelFaker.Generate();

        int rowsAffected = await repository.InsertAsync(newProductModel);
        Assert.Equal(1, rowsAffected);

        int newCount = await repository.CountAsync();
        Assert.Equal(count + 1, newCount);
    }

    [Fact]
    public async Task InsertManyAsync()
    {
        int count = await repository.CountAsync();
        var newProductModels = productModelFaker.Generate(10);

        int rowsAffected = await repository.InsertAsync(newProductModels);
        Assert.Equal(10, rowsAffected);

        int newCount = await repository.CountAsync();
        Assert.Equal(count + 10, newCount);
    }

    #endregion Insert

    #region Update

    [Fact]
    public void Update()
    {
        var randomProduct = new Random().NextFrom(products);
        var entity = repository.FindOne(randomProduct.ProductModelId);

        string newName = "Foo Bar Baz";
        entity.Name = newName;
        int rowsAffected = repository.Update(entity);
        Assert.Equal(1, rowsAffected);

        var entityAgain = repository.FindOne(randomProduct.ProductModelId);
        Assert.Equal(newName, entityAgain.Name);
    }

    [Fact]
    public void UpdateMany()
    {
        var random = new Random();

        var randomProductIds = Enumerable.Range(1, 10)
            .Select(x => random.NextFrom(products))
            .Select(x => x.ProductModelId)
            .ToList();

        string namePrefix = "Foo Bar Baz";

        var entities = repository.Find(new SearchOptions<ProductModel>
        {
            Query = x => randomProductIds.Contains(x.ProductModelId)
        });
        int count1 = entities.Count();
        foreach (var entity in entities)
        {
            entity.Name = namePrefix;
        }

        int rowsAffected = repository.Update(entities);
        Assert.Equal(count1, rowsAffected);

        var entitiesAgain = repository.Find(new SearchOptions<ProductModel>
        {
            Query = x => x.Name == namePrefix
        });
        Assert.Equal(count1, entitiesAgain.Count());
    }

    [Fact]
    public async Task UpdateAsync()
    {
        var randomProduct = new Random().NextFrom(products);
        var entity = await repository.FindOneAsync(randomProduct.ProductModelId);

        string newName = "Foo Bar Baz";
        entity.Name = newName;
        int rowsAffected = await repository.UpdateAsync(entity);
        Assert.Equal(1, rowsAffected);

        var entityAgain = await repository.FindOneAsync(randomProduct.ProductModelId);
        Assert.Equal(newName, entityAgain.Name);
    }

    [Fact]
    public async Task UpdateManyAsync()
    {
        var random = new Random();

        var randomProductIds = Enumerable.Range(1, 10)
            .Select(x => random.NextFrom(products))
            .Select(x => x.ProductModelId)
            .ToList();

        string namePrefix = "Foo Bar Baz";

        var entities = await repository.FindAsync(new SearchOptions<ProductModel>
        {
            Query = x => randomProductIds.Contains(x.ProductModelId)
        });
        int count1 = entities.Count();
        foreach (var entity in entities)
        {
            entity.Name = namePrefix;
        }

        int rowsAffected = await repository.UpdateAsync(entities);
        Assert.Equal(count1, rowsAffected);

        var entitiesAgain = await repository.FindAsync(new SearchOptions<ProductModel>
        {
            Query = x => x.Name == namePrefix
        });
        Assert.Equal(count1, entitiesAgain.Count());
    }

    // Relies on Z.EntityFramework.Plus which does not seem to support in memory db.
    //[Fact]
    //public void Update_Factory()
    //{
    //    var date = new DateTime(2000, 1, 1);
    //    repository.Update(x => new ProductModel { ModifiedDate = date });

    //    int expected = repository.Count();
    //    int actual = repository.Count(x => x.ModifiedDate == date);

    //    Assert.Equal(expected, actual);
    //}

    // Relies on Z.EntityFramework.Plus which does not seem to support in memory db.
    //[Fact]
    //public void Update_Predicate_And_Factory()
    //{
    //    var date = new DateTime(2000, 1, 1);
    //    repository.Update(x => x.Name.StartsWith("M"), x => new ProductModel { ModifiedDate = date });

    //    int expected = repository.Count(x => x.Name.StartsWith("M"));
    //    int actual = repository.Count(x => x.Name.StartsWith("M") && x.ModifiedDate == date);

    //    Assert.Equal(expected, actual);
    //}

    // Relies on Z.EntityFramework.Plus which does not seem to support in memory db.
    //[Fact]
    //public void Update_Query_And_Factory()
    //{
    //    var date = new DateTime(2000, 1, 1);
    //    var query = repository.Find(x => x.Name.StartsWith("M")).AsQueryable();
    //    repository.Update(query, x => new ProductModel { ModifiedDate = date });

    //    int expected = repository.Count(x => x.Name.StartsWith("M"));
    //    int actual = repository.Count(x => x.Name.StartsWith("M") && x.ModifiedDate == date);

    //    Assert.Equal(expected, actual);
    //}

    // Relies on Z.EntityFramework.Plus which does not seem to support in memory db.
    //[Fact]
    //public async Task UpdateAsync_Factory()
    //{
    //    var date = new DateTime(2000, 1, 1);
    //    await repository.UpdateAsync(x => new ProductModel { ModifiedDate = date });

    //    int expected = await repository.CountAsync();
    //    int actual = await repository.CountAsync(x => x.ModifiedDate == date);

    //    Assert.Equal(expected, actual);
    //}

    // Relies on Z.EntityFramework.Plus which does not seem to support in memory db.
    //[Fact]
    //public async Task UpdateAsync_Predicate_And_Factory()
    //{
    //    var date = new DateTime(2000, 1, 1);
    //    await repository.UpdateAsync(x => x.Name.StartsWith("M"), x => new ProductModel { ModifiedDate = date });

    //    int expected = await repository.CountAsync(x => x.Name.StartsWith("M"));
    //    int actual = await repository.CountAsync(x => x.Name.StartsWith("M") && x.ModifiedDate == date);

    //    Assert.Equal(expected, actual);
    //}

    // Relies on Z.EntityFramework.Plus which does not seem to support in memory db.
    //[Fact]
    //public async Task UpdateAsync_Query_And_Factory()
    //{
    //    var date = new DateTime(2000, 1, 1);
    //    var query = (await repository.FindAsync(x => x.Name.StartsWith("M"))).AsQueryable();
    //    await repository.UpdateAsync(query, x => new ProductModel { ModifiedDate = date });

    //    int expected = await repository.CountAsync(x => x.Name.StartsWith("M"));
    //    int actual = await repository.CountAsync(x => x.Name.StartsWith("M") && x.ModifiedDate == date);

    //    Assert.Equal(expected, actual);
    //}

    #endregion Update

    #region Dispose Pattern

    protected virtual void Dispose(bool disposing)
    {
        if (!isDisposed)
        {
            if (disposing)
            {
                using var context = contextFactory.GetContext();
                context.Database.EnsureDeleted(); // Necessary to reset EF in-memory provider between tests..
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