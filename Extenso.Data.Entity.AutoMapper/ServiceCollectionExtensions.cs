using Microsoft.Extensions.DependencyInjection;

namespace Extenso.Data.Entity.AutoMapper;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        /// <summary>
        /// Adds the Mapped Entity Framework repository services to the service collection.
        /// </summary>
        /// <param name="services">The service collection to add the services to.</param>
        /// <returns>The updated service collection.</returns>
        public IServiceCollection AddAutoMapperEntityFrameworkRepository()
        {
            services.AddScoped(typeof(IEntityModelMapper<,>), typeof(AutoMapperEntityModelMapper<,>));
            services.AddScoped(typeof(IMappedRepository<,>), typeof(MappedEntityFrameworkRepository<,>));
            return services;
        }
    }
}