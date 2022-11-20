using Extenso.TestLib.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Extenso.Data.Entity.Tests
{
    public class DbContextExtensionsTests : IDisposable
    {
        private bool isDisposed;

        private readonly AdventureWorks2019Context context;

        public DbContextExtensionsTests()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<AdventureWorks2019Context>();
            optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            context = new AdventureWorks2019Context(optionsBuilder.Options);
        }

        [Fact]
        public void CreateParameter()
        {
            var expected = new SqlParameter
            {
                ParameterName = "Foo",
                Value = 999
            };

            var actual = context.CreateParameter("Foo", 999);

            Assert.True(actual != null, "Parameter was null");
            Assert.True(actual is SqlParameter, "Parameter was not of the expected type");
            Assert.Equal(actual.ParameterName, expected.ParameterName);
            Assert.Equal(actual.Value, expected.Value);
        }

        [Fact]
        public void ExecuteScalar_Int()
        {
            int expected = 19972;
            int actual = context.ExecuteScalar<int>("SELECT COUNT(*) FROM [Person].[Person]");
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ExecuteScalar_String()
        {
            string expected = "Sánchez";
            string actual = context.ExecuteScalar<string>("SELECT [LastName] FROM [Person].[Person] WHERE [BusinessEntityID] = 1");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ExecuteStoredProcedure()
        {
            var dataSet = context.ExecuteStoredProcedure("[dbo].[uspGetEmployeeManagers]", new[]
            {
                context.CreateParameter("BusinessEntityID", 3)
            });

            Assert.True(dataSet.Tables.Count == 1, "There should be 1 table in the data set.");

            var table = dataSet.Tables[0];

            Assert.True(table.Rows.Count == 1, "There should be 1 row in the table.");
        }

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