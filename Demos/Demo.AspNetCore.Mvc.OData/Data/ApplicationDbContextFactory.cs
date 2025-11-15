using Extenso.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace Demo.Extenso.AspNetCore.Mvc.OData.Data;

public class ApplicationDbContextFactory : IDbContextFactory
{
    public ApplicationDbContextFactory(IConfiguration configuration)
    {
    }

    private DbContextOptions<ApplicationDbContext> Options
    {
        get
        {
            if (field is null)
            {
                var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                optionsBuilder.UseInMemoryDatabase("DemoDb");
                field = optionsBuilder.Options;
            }
            return field;
        }
    }

    public DbContext GetContext() => new ApplicationDbContext(Options);

    public DbContext GetContext(string connectionString)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseInMemoryDatabase(connectionString);
        return new ApplicationDbContext(optionsBuilder.Options);
    }
}