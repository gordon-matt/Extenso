using Microsoft.Extensions.DependencyInjection;

namespace Extenso.Data.Entity;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the Entity Framework repository services to the service collection.
    /// </summary>
    /// <param name="services">The service collection to add the services to.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddEntityFrameworkRepository(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(EntityFrameworkRepository<>));
        return services;
    }

    /// <summary>
    /// Adds the Mapped Entity Framework repository services to the service collection.
    /// </summary>
    /// <param name="services">The service collection to add the services to.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddExtensoMapperEntityFrameworkRepository(this IServiceCollection services)
    {
        services.AddScoped(typeof(IEntityModelMapper<,>), typeof(ExtensoEntityModelMapper<,>));
        services.AddScoped(typeof(IMappedRepository<,>), typeof(MappedEntityFrameworkRepository<,>));
        return services;
    }
}