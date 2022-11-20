﻿using System.Diagnostics.Metrics;
using System.Drawing;
using System.Linq.Expressions;
using Bogus;
using Extenso.TestLib.Data;
using Extenso.TestLib.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace Extenso.Data.Entity.Tests
{
    public class EntityFrameworkRepositoryTests : IDisposable
    {
        private InMemoryAdventureWorks2019ContextFactory contextFactory;
        private IRepository<ProductModel> repository;
        private ICollection<ProductModel> productModels;
        private ICollection<Product> products;
        private bool isDisposed;

        public EntityFrameworkRepositoryTests()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<AdventureWorks2019Context>();
            optionsBuilder.UseInMemoryDatabase("AdventureWorks2019");
            using var context = new AdventureWorks2019Context(optionsBuilder.Options);

            var productModelFaker = new Faker<ProductModel>()
                .RuleFor(x => x.Name, x => x.Commerce.Department())
                .RuleFor(x => x.CatalogDescription, x => x.Commerce.ProductDescription())
                .RuleFor(x => x.Instructions, x => x.Lorem.Paragraph())
                .RuleFor(x => x.Rowguid, x => x.Random.Guid())
                .RuleFor(x => x.ModifiedDate, x => x.Date.Between(DateTime.Today.AddYears(-10), DateTime.Today.AddDays(-1)));

            productModels = productModelFaker.Generate(100);
            context.ProductModels.AddRange(productModels);
            context.SaveChanges();

            var productFaker = new Faker<Product>()
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

            int actual = repository.Find(include => include.Products)
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

            int actual = repository.Find(x => x.Name.StartsWith(firstLetter), include => include.Products)
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

            var query = await repository.FindAsync(include => include.Products);

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

            var query = await repository.FindAsync(x => x.Name.StartsWith(firstLetter), include => include.Products);

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
            var entity = repository.FindOne(x => x.Name == productModel.Name, include => include.Products);
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
            var entity = await repository.FindOneAsync(x => x.Name == productModel.Name, include => include.Products);
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

            var entities = repository.Find().Take(5);
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

            var entities = (await repository.FindAsync()).Take(5);
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
    }
}