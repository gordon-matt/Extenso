using System;
using Microsoft.AspNetCore.Routing;

namespace Extenso.AspNetCore.OData
{
    public interface IODataRegistrar
    {
        void Register(IRouteBuilder routes, IServiceProvider services);
    }
}