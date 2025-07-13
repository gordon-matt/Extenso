using System.Drawing;
using AutoMapper;
using Bogus;
using Extenso.Data.Entity.AutoMapper;
using Extenso.TestLib.Data;
using Extenso.TestLib.Data.Entities;
using Extenso.TestLib.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Z.EntityFramework.Plus;

namespace Extenso.Data.Entity.Tests;

[Collection("EntityFrameworkRepositoryTests")]
public class AutoMapperEntityFrameworkRepositoryTests : IDisposable
{
    private readonly IMapper mapper;
    private readonly InMemoryAdventureWorks2019ContextFactory contextFactory;
    private readonly IMappedRepository<ProductModelViewModel, ProductModel> repository;
    private readonly ICollection<ProductModelViewModel> productModels;
    private readonly ICollection<ProductViewModel> products;
    private bool isDisposed;

    private readonly Faker<ProductModelViewModel> productModelFaker;
    private readonly Faker<ProductViewModel> productFaker;

    public AutoMapperEntityFrameworkRepositoryTests()
    {
        mapper = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<ProductViewModel, Product>();
            cfg.CreateMap<Product, ProductViewModel>();

            cfg.CreateMap<ProductModel, ProductModelViewModel>();
            cfg.CreateMap<ProductModelViewModel, ProductModel>();

            cfg.CreateMap<ProductSubcategory, ProductSubcategoryViewModel>();
            cfg.CreateMap<ProductSubcategoryViewModel, ProductSubcategory>();

            // Configure anonymous type mapping
            cfg.CreateMap<ProductModel, object>()
                .ConvertUsing((src, dest) => new { src.Name, src.CatalogDescription });
        }).CreateMapper();

        var optionsBuilder = new DbContextOptionsBuilder<AdventureWorks2019Context>();
        optionsBuilder.UseInMemoryDatabase("AdventureWorks2019");
        using var context = new AdventureWorks2019Context(optionsBuilder.Options);

        productModelFaker = new Faker<ProductModelViewModel>()
            .RuleFor(x => x.Name, x => x.Commerce.Department())
            .RuleFor(x => x.CatalogDescription, x => x.Commerce.ProductDescription())
            .RuleFor(x => x.Instructions, x => x.Lorem.Paragraph())
            .RuleFor(x => x.Rowguid, x => x.Random.Guid())
            .RuleFor(x => x.ModifiedDate, x => x.Date.Between(DateTime.Today.AddYears(-10), DateTime.Today.AddDays(-1)));

        productModels = productModelFaker.Generate(100);
        context.ProductModels.AddRange(productModels.Select(x => mapper.Map<ProductModel>(x)));
        context.SaveChanges();

        // Get them again, so we have IDs:
        productModels = context.ProductModels
            .ToList()
            .Select(x => mapper.Map<ProductModelViewModel>(x))
            .ToList();

        productFaker = new Faker<ProductViewModel>()
            .RuleFor(x => x.ProductModelId, x => x.PickRandom(productModels).ProductModelId)
            .RuleFor(x => x.Name, x => x.Commerce.ProductName())
            .RuleFor(x => x.ProductNumber, x => x.Commerce.Ean13())
            .RuleFor(x => x.Color, x => x.PickRandom(EnumExtensions.GetValues<KnownColor>()).ToString())
            .RuleFor(x => x.FinishedGoodsFlag, x => x.Random.Bool())
            .RuleFor(x => x.MakeFlag, x => x.Random.Bool())
            .RuleFor(x => x.Rowguid, x => x.Random.Guid())
            .RuleFor(x => x.ModifiedDate, x => x.Date.Between(DateTime.Today.AddYears(-10), DateTime.Today.AddDays(-1)))
            .RuleFor(x => x.ProductSubcategory, x => new ProductSubcategoryViewModel
            {
                Name = x.Commerce.Categories(1).First(),
                Rowguid = x.Random.Guid(),
                ModifiedDate = x.Date.Between(DateTime.Today.AddYears(-10), DateTime.Today.AddDays(-1)),
                ProductCategoryId = x.Random.Int(),
            });

        products = productFaker.Generate(100);
        context.Products.AddRange(products.Select(x => mapper.Map<Product>(x)));
        context.SaveChanges();

        // Get them again, so we have IDs:
        products = context.Products
            .ToList()
            .Select(x => mapper.Map<ProductViewModel>(x))
            .ToList();

        contextFactory = new InMemoryAdventureWorks2019ContextFactory();

        var entityModelMapper = new AutoMapperEntityModelMapper<ProductModel, ProductModelViewModel>(mapper);
        repository = new MappedEntityFrameworkRepository<ProductModelViewModel, ProductModel>(contextFactory, Mock.Of<ILoggerFactory>(), entityModelMapper);
    }

    [Fact]
    public void Connection_Query()
    {
        var randomProduct = new Random().NextFrom(products);

        int expected = products.Count(x => x.ProductModelId == randomProduct.ProductModelId);

        using var connection = repository.OpenConnection();
        int actual = connection
            .Query(
                x => x.ProductModelId == randomProduct.ProductModelId,
                include => include.Products)
            .SelectMany(x => x.Products)
            .Count();

        Assert.Equal(expected, actual);
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

        var query = await repository
            .FindAsync(new SearchOptions<ProductModel>
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

    [Fact]
    public void Find_With_Projection()
    {
        var randomProduct = new Random().NextFrom(products);
        var productModel = productModels.First(x => x.ProductModelId == randomProduct.ProductModelId);

        var result = repository.Find(
            new SearchOptions<ProductModel>
            {
                Query = x => x.ProductModelId == productModel.ProductModelId
            },
            x => new ProductModelProjection { Name = x.Name, CatalogDescription = x.CatalogDescription }
        ).First();

        Assert.NotNull(result);
        Assert.Equal(productModel.Name, result.Name);
        Assert.Equal(productModel.CatalogDescription, result.CatalogDescription);
    }

    [Fact]
    public async Task FindAsync_With_Projection()
    {
        var randomProduct = new Random().NextFrom(products);
        var productModel = productModels.First(x => x.ProductModelId == randomProduct.ProductModelId);

        var result = (await repository.FindAsync(
            new SearchOptions<ProductModel>
            {
                Query = x => x.ProductModelId == productModel.ProductModelId
            },
            x => new ProductModelProjection { Name = x.Name, CatalogDescription = x.CatalogDescription }
        )).First();

        Assert.NotNull(result);
        Assert.Equal(productModel.Name, result.Name);
        Assert.Equal(productModel.CatalogDescription, result.CatalogDescription);
    }

    [Fact]
    public void FindOne_With_Projection()
    {
        var randomProduct = new Random().NextFrom(products);
        var productModel = productModels.First(x => x.ProductModelId == randomProduct.ProductModelId);

        var result = repository.FindOne(
            new SearchOptions<ProductModel>
            {
                Query = x => x.ProductModelId == productModel.ProductModelId
            },
            x => new ProductModelProjection { Name = x.Name, CatalogDescription = x.CatalogDescription }
        );

        Assert.NotNull(result);
        Assert.Equal(productModel.Name, result.Name);
        Assert.Equal(productModel.CatalogDescription, result.CatalogDescription);
    }

    [Fact]
    public async Task FindOneAsync_With_Projection()
    {
        var randomProduct = new Random().NextFrom(products);
        var productModel = productModels.First(x => x.ProductModelId == randomProduct.ProductModelId);

        var result = await repository.FindOneAsync(
            new SearchOptions<ProductModel>
            {
                Query = x => x.ProductModelId == productModel.ProductModelId
            },
            x => new ProductModelProjection { Name = x.Name, CatalogDescription = x.CatalogDescription }
        );

        Assert.NotNull(result);
        Assert.Equal(productModel.Name, result.Name);
        Assert.Equal(productModel.CatalogDescription, result.CatalogDescription);
    }

    [Fact]
    public void Find_With_OrderBy()
    {
        var results = repository.Find(new SearchOptions<ProductModel>
        {
            OrderBy = query => query.OrderBy(x => x.Name)
        }).ToList();

        var expected = productModels.OrderBy(x => x.Name).ToList();
        Assert.Equal(expected.Count, results.Count);
        for (int i = 0; i < expected.Count; i++)
        {
            Assert.Equal(expected[i].Name, results[i].Name);
        }
    }

    [Fact]
    public void Find_With_OrderByDescending()
    {
        var results = repository.Find(new SearchOptions<ProductModel>
        {
            OrderBy = query => query.OrderByDescending(x => x.Name)
        }).ToList();

        var expected = productModels.OrderByDescending(x => x.Name).ToList();
        Assert.Equal(expected.Count, results.Count);
        for (int i = 0; i < expected.Count; i++)
        {
            Assert.Equal(expected[i].Name, results[i].Name);
        }
    }

    [Fact]
    public void Find_With_MultiLevel_OrderBy()
    {
        var results = repository.Find(new SearchOptions<ProductModel>
        {
            OrderBy = query => query
                .OrderBy(x => x.Name)
                .ThenByDescending(x => x.ModifiedDate)
        }).ToList();

        var expected = productModels
            .OrderBy(x => x.Name)
            .ThenByDescending(x => x.ModifiedDate)
            .ToList();

        Assert.Equal(expected.Count, results.Count);
        for (int i = 0; i < expected.Count; i++)
        {
            Assert.Equal(expected[i].Name, results[i].Name);
            Assert.Equal(expected[i].ModifiedDate, results[i].ModifiedDate);
        }
    }

    [Fact]
    public void Find_With_MultiLevel_Include()
    {
        var randomProduct = new Random().NextFrom(products);
        var productModel = productModels.First(x => x.ProductModelId == randomProduct.ProductModelId);

        var result = repository.Find(new SearchOptions<ProductModel>
        {
            Query = x => x.ProductModelId == productModel.ProductModelId,
            Include = query => query
                .Include(x => x.Products)
                .ThenInclude(x => x.ProductSubcategory)
        }).First();

        Assert.NotNull(result);
        Assert.NotNull(result.Products);
        Assert.NotEmpty(result.Products);
        Assert.NotNull(result.Products.First().ProductSubcategory);
    }

    [Fact]
    public async Task FindAsync_With_MultiLevel_Include()
    {
        var randomProduct = new Random().NextFrom(products);
        var productModel = productModels.First(x => x.ProductModelId == randomProduct.ProductModelId);

        var result = (await repository.FindAsync(new SearchOptions<ProductModel>
        {
            Query = x => x.ProductModelId == productModel.ProductModelId,
            Include = query => query
                .Include(x => x.Products)
                .ThenInclude(x => x.ProductSubcategory)
        })).First();

        Assert.NotNull(result);
        Assert.NotNull(result.Products);
        Assert.NotEmpty(result.Products);
        Assert.NotNull(result.Products.First().ProductSubcategory);
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
        var model = repository.FindOne(randomProductModel.ProductModelId);
        Assert.NotNull(model);

        int rowsAffected = repository.Delete(model);
        Assert.Equal(1, rowsAffected);

        int newCount = repository.Count();
        Assert.Equal(count - 1, newCount);
    }

    [Fact]
    public void DeleteMany()
    {
        int count = repository.Count();

        var models = repository.Find(new SearchOptions<ProductModel>
        {
            Query = x => true
        }).Take(5);
        Assert.NotEmpty(models);

        int rowsAffected = repository.Delete(models);
        Assert.Equal(5, rowsAffected);

        int newCount = repository.Count();
        Assert.Equal(count - 5, newCount);
    }

    [Fact]
    public async Task DeleteAsync()
    {
        int count = await repository.CountAsync();

        var randomProductModel = new Random().NextFrom(productModels);
        var model = await repository.FindOneAsync(randomProductModel.ProductModelId);
        Assert.NotNull(model);

        int rowsAffected = await repository.DeleteAsync(model);
        Assert.Equal(1, rowsAffected);

        int newCount = await repository.CountAsync();
        Assert.Equal(count - 1, newCount);
    }

    [Fact]
    public async Task DeleteManyAsync()
    {
        int count = await repository.CountAsync();

        var models = (await repository.FindAsync(new SearchOptions<ProductModel>
        {
            Query = x => true
        })).Take(5);
        Assert.NotEmpty(models);

        int rowsAffected = await repository.DeleteAsync(models);
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

        var insertedModel = repository.Insert(newProductModel);
        Assert.True(insertedModel.ProductModelId > 0);

        int newCount = repository.Count();
        Assert.Equal(count + 1, newCount);
    }

    [Fact]
    public void InsertMany()
    {
        int count = repository.Count();
        var newProductModels = productModelFaker.Generate(10);

        var insertedModels = repository.Insert(newProductModels);
        Assert.All(insertedModels, x => Assert.True(x.ProductModelId > 0));

        int newCount = repository.Count();
        Assert.Equal(count + 10, newCount);
    }

    [Fact]
    public async Task InsertAsync()
    {
        int count = await repository.CountAsync();
        var newProductModel = productModelFaker.Generate();

        var insertedModel = await repository.InsertAsync(newProductModel);
        Assert.True(insertedModel.ProductModelId > 0);

        int newCount = await repository.CountAsync();
        Assert.Equal(count + 1, newCount);
    }

    [Fact]
    public async Task InsertManyAsync()
    {
        int count = await repository.CountAsync();
        var newProductModels = productModelFaker.Generate(10);

        var insertedModels = await repository.InsertAsync(newProductModels);
        Assert.All(insertedModels, x => Assert.True(x.ProductModelId > 0));

        int newCount = await repository.CountAsync();
        Assert.Equal(count + 10, newCount);
    }

    #endregion Insert

    #region Update

    [Fact]
    public void Update()
    {
        var randomProduct = new Random().NextFrom(products);
        var model = repository.FindOne(randomProduct.ProductModelId);

        string newName = "Foo Bar Baz";
        model.Name = newName;
        var updatedModel = repository.Update(model);
        Assert.True(updatedModel.Name == newName);

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

        string newName = "Foo Bar Baz";

        var models = repository.Find(new SearchOptions<ProductModel>
        {
            Query = x => randomProductIds.Contains(x.ProductModelId)
        });
        int count1 = models.Count();
        foreach (var model in models)
        {
            model.Name = newName;
        }

        var updatedModels = repository.Update(models);
        Assert.All(updatedModels, x => Assert.True(x.Name == newName));

        var entitiesAgain = repository.Find(new SearchOptions<ProductModel>
        {
            Query = x => x.Name == newName
        });
        Assert.Equal(count1, entitiesAgain.Count());
    }

    [Fact]
    public async Task UpdateAsync()
    {
        var randomProduct = new Random().NextFrom(products);
        var model = await repository.FindOneAsync(randomProduct.ProductModelId);

        string newName = "Foo Bar Baz";
        model.Name = newName;
        var updatedModel = await repository.UpdateAsync(model);
        Assert.True(updatedModel.Name == newName);

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

        string newName = "Foo Bar Baz";

        var models = await repository.FindAsync(new SearchOptions<ProductModel>
        {
            Query = x => randomProductIds.Contains(x.ProductModelId)
        });
        int count1 = models.Count();
        foreach (var model in models)
        {
            model.Name = newName;
        }

        var updatedModels = await repository.UpdateAsync(models);
        Assert.All(updatedModels, x => Assert.True(x.Name == newName));

        var entitiesAgain = await repository.FindAsync(new SearchOptions<ProductModel>
        {
            Query = x => x.Name == newName
        });
        Assert.Equal(count1, entitiesAgain.Count());
    }

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

    private class ProductModelProjection
    {
        public string Name { get; set; }

        public string CatalogDescription { get; set; }
    }
}