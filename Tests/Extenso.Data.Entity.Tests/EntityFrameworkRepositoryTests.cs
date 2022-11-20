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
        private bool isDisposed;

        private readonly AdventureWorks2019Context context;
        private readonly IRepository<ProductModel> repository;

        public EntityFrameworkRepositoryTests()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<AdventureWorks2019Context>();
            optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            context = new AdventureWorks2019Context(optionsBuilder.Options);

            var contextFactory = new AdventureWorks2019ContextFactory(config);
            repository = new EntityFrameworkRepository<ProductModel>(contextFactory, Mock.Of<ILoggerFactory>());
        }

        #region Count

        [Fact]
        public void Count()
        {
            int expected = 128;
            int actual = repository.Count();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Count_With_Predicate()
        {
            int expected = 36;
            int actual = repository.Count(x => x.Name.StartsWith("M"));
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task CountAsync()
        {
            int expected = 128;
            int actual = await repository.CountAsync();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task CountAsync_With_Predicate()
        {
            int expected = 36;
            int actual = await repository.CountAsync(x => x.Name.StartsWith("M"));
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void LongCount()
        {
            long expected = 128;
            long actual = repository.LongCount();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void LongCount_With_Predicate()
        {
            long expected = 36;
            long actual = repository.LongCount(x => x.Name.StartsWith("M"));
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task LongCountAsync()
        {
            long expected = 128;
            long actual = await repository.LongCountAsync();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task LongCountAsync_With_Predicate()
        {
            long expected = 36;
            long actual = await repository.LongCountAsync(x => x.Name.StartsWith("M"));
            Assert.Equal(expected, actual);
        }

        #endregion Count

        #region Dispose Pattern

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    context?.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                isDisposed = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~DbConnectionExtensionsTests()
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
}