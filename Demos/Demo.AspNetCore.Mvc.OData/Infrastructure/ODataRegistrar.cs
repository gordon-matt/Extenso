﻿using System;
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
            ODataModelBuilder builder = new ODataConventionModelBuilder(services);

            builder.EntitySet<Person>("PersonApi");

            routes.MapODataServiceRoute("OData", "odata", builder.GetEdmModel());
        }
    }
}