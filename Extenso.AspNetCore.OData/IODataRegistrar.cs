using System;
using Microsoft.AspNetCore.Routing;

namespace Extenso.AspNetCore.OData
{
    /// <summary>
    /// Defines a contract for an OData registrar in an application. An OData registrar specifies
    /// the OData routes for an application.
    /// </summary>
    public interface IODataRegistrar
    {
        /// <summary>
        /// Registers one or more OData routes for use in the application.
        /// </summary>
        /// <param name="routes">An instance of Microsoft.AspNetCore.Routing.IRouteBuilder to add the OData routes to.</param>
        /// <param name="services">The System.IServiceProvider that provides access to the application's service container</param>
        void Register(IRouteBuilder routes, IServiceProvider services);

        /// <summary>
        /// Registers one or more OData endpoints for use in the application.
        /// </summary>
        /// <param name="endpoints">An instance of Microsoft.AspNetCore.Routing.IEndpointRouteBuilder to add the OData endpoints to.</param>
        /// <param name="services">The System.IServiceProvider that provides access to the application's service container</param>
        void Register(IEndpointRouteBuilder endpoints, IServiceProvider services);
    }
}