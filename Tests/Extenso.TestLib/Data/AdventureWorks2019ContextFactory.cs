using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Extenso.TestLib.Data
{
    public class AdventureWorks2019ContextFactory : IDbContextFactory
    {
        private readonly IConfiguration configuration;

        public AdventureWorks2019ContextFactory(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        private DbContextOptions<AdventureWorks2019Context> options;

        private DbContextOptions<AdventureWorks2019Context> Options
        {
            get
            {
                if (options == null)
                {
                    var optionsBuilder = new DbContextOptionsBuilder<AdventureWorks2019Context>();
                    optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                    options = optionsBuilder.Options;
                }
                return options;
            }
        }

        public DbContext GetContext()
        {
            return new AdventureWorks2019Context(Options);
        }

        public DbContext GetContext(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AdventureWorks2019Context>();
            optionsBuilder.UseSqlServer(connectionString);
            return new AdventureWorks2019Context(optionsBuilder.Options);
        }
    }

    public class InMemoryAdventureWorks2019ContextFactory : IDbContextFactory
    {
        private DbContextOptions<AdventureWorks2019Context> options;

        private DbContextOptions<AdventureWorks2019Context> Options
        {
            get
            {
                if (options == null)
                {
                    var optionsBuilder = new DbContextOptionsBuilder<AdventureWorks2019Context>();
                    optionsBuilder.UseInMemoryDatabase("AdventureWorks2019");
                    options = optionsBuilder.Options;
                }
                return options;
            }
        }

        public DbContext GetContext()
        {
            return new AdventureWorks2019Context(Options);
        }

        public DbContext GetContext(string databaseName)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AdventureWorks2019Context>();
            optionsBuilder.UseInMemoryDatabase(databaseName);
            return new AdventureWorks2019Context(optionsBuilder.Options);
        }
    }
}