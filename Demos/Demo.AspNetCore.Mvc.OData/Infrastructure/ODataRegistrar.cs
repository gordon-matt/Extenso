using System;
using Demo.Extenso.AspNetCore.Mvc.OData.Data.Domain;
using Extenso.AspNetCore.OData;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Routing;

namespace Demo.Extenso.AspNetCore.Mvc.OData.Infrastructure
{
    public class ODataRegistrar : IODataRegistrar
    {
        public void Register(IRouteBuilder routes, IServiceProvider services)
        {
            var builder = GetBuilder(services);
            routes.MapODataServiceRoute("OData", "odata", builder.GetEdmModel());
        }

        public void Register(IEndpointRouteBuilder endpoints, IServiceProvider services)
        {
            var builder = GetBuilder(services);
            endpoints.MapODataRoute("OData", "odata", builder.GetEdmModel());
        }

        private ODataModelBuilder GetBuilder(IServiceProvider services)
        {
            ODataModelBuilder builder = new ODataConventionModelBuilder(services);
            builder.EntitySet<Person>("PersonApi");
            return builder;
        }
    }
}