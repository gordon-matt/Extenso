using Microsoft.Extensions.DependencyInjection;

namespace Extenso.Data.Entity.AutoMapper;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the Mapped Entity Framework repository services to the service collection.
    /// </summary>
    /// <param name="services">The service collection to add the services to.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddAutoMapperEntityFrameworkRepository(this IServiceCollection services)
    {
        services.AddScoped(typeof(IEntityModelMapper<,>), typeof(AutoMapperEntityModelMapper<,>));
        services.AddScoped(typeof(IMappedRepository<,>), typeof(MappedEntityFrameworkRepository<,>));
        return services;
    }
}