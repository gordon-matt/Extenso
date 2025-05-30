[![NuGet](https://img.shields.io/nuget/v/Extenso.Data.Entity?style=flat-square&logo=nuget&label=Version)](https://www.nuget.org/packages/Extenso.Data.Entity)
[![NuGet](https://img.shields.io/nuget/dt/Extenso.Data.Entity?style=flat-square&logo=nuget&label=Downloads)](https://www.nuget.org/packages/Extenso.Data.Entity)
[![Donate](https://img.shields.io/badge/Donate-PayPal-green.svg)](https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=gordon_matt%40live%2ecom&lc=AU&currency_code=AUD&bn=PP%2dDonationsBF%3abtn_donateCC_LG%2egif%3aNonHosted)

<img src="https://github.com/gordon-matt/Extenso/blob/master/_Misc/ExtensoLogo.png" alt="Logo" width="250" />

# Extenso.Data.Entity

## Intro
This package contains a generic `IRepository<TEntity>` and `IMappedRepository<TModel, TEntity>` implementations for Entity Framework,
as well as other data-related extension methods and helper classes.

## Getting Started

The first thing you're going to need to do is have an IDbContextFactory implementation. Example:

```csharp
public class ApplicationDbContextFactory : IDbContextFactory
{
    private readonly IConfiguration configuration;

    public ApplicationDbContextFactory(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    private DbContextOptions<ApplicationDbContext> options;

    private DbContextOptions<ApplicationDbContext> Options
    {
        get
        {
            if (options == null)
            {
                var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                options = optionsBuilder.Options;
            }
            return options;
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
```

Next, you can register the `EntityFrameworkRepository` in your dependency injection container. An example:

```csharp
// Regular direct to entity repository:
services.AddScoped(typeof(IRepository<>), typeof(EntityFrameworkRepository<>));

// Mapped repository, which maps between entities and models:
services.AddScoped(typeof(IMappedRepository<,>), typeof(ExtensoMapperEntityFrameworkRepository<,>));

// Or using the Extenso.Data.Entity.AutoMapper package, you could do this:
services.AddScoped(typeof(IMappedRepository<,>), typeof(AutoMapperEntityFrameworkRepository<,>));
```

Now just inject the repository into your classes and use it:

```csharp
public class BlogService
{
    private readonly IRepository<BlogCategory> blogCategoryService;
    private readonly IRepository<BlogPost> blogPostService;
    private readonly IRepository<BlogTag> blogTagService;
    
    public BlogCategoryService(
        IRepository<BlogCategory> blogCategoryService,
        IRepository<BlogPost> blogPostService,
        IRepository<BlogTag> blogTagService)
    {
        this.blogCategoryService = blogCategoryService;
        this.blogPostService = blogPostService;
        this.blogTagService = blogTagService;
    }

    public async Task<IEnumerable<BlogCategory>> GetCategoriesAsync(int tenantId) =>
        await blogCategoryService.FindAsync(new SearchOptions<BlogCategory>
        {
            Query = x =>
                x.TenantId == tenantId,

            Include = query => query
                .Include(x => x.Posts)
                .ThenInclude(x => x.Tags),

            OrderBy = query => query.OrderBy(x => x.Name)
        });

    // etc...
}
```

All CRUD operations are supported, including paging, searching, and sorting.
Even projections are supported via method overloads of `Find()`, `FindAsync()`, `FindOne()`, and `FindOneAsync()`.

## Donate
If you find this project helpful, consider buying me a cup of coffee.  :-)

[![PayPal](https://img.shields.io/badge/PayPal-003087?logo=paypal&logoColor=fff)](https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=gordon_matt%40live%2ecom&lc=AU&currency_code=AUD&bn=PP%2dDonationsBF%3abtn_donateCC_LG%2egif%3aNonHosted)
[![Bitcoin](https://img.shields.io/badge/Bitcoin-FF9900?logo=bitcoin&logoColor=white)](bitcoin:1EeDfbcqoEaz6bbcWsymwPbYv4uyEaZ3Lp)
[![Ethereum](https://img.shields.io/badge/Ethereum-3C3C3D?logo=ethereum&logoColor=white)](ethereum:0x277552efd6ea9ca9052a249e781abf1719ea9414)
[![Litecoin](https://img.shields.io/badge/Litecoin-A6A9AA?logo=litecoin&logoColor=white)](litecoin:LRUP8hukWGXRrcPK6Tm7iUp9vPvnNNt3uz)
