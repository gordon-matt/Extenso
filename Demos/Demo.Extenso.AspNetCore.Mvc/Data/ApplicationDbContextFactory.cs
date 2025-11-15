using Extenso.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace Demo.Extenso.AspNetCore.Mvc.Data;

public class ApplicationDbContextFactory : IDbContextFactory
{
    private readonly IConfiguration configuration;

    public ApplicationDbContextFactory(IConfiguration configuration)
    {
        this.configuration = configuration;
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