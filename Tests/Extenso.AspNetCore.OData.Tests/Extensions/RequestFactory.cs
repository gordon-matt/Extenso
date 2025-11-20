//-----------------------------------------------------------------------------
// <copyright file="RequestFactory.cs" company=".NET Foundation">
//      Copyright (c) .NET Foundation and Contributors. All rights reserved.
//      See License.txt in the project root for license information.
// </copyright>
//------------------------------------------------------------------------------

using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.OData.Abstracts;
using Microsoft.AspNetCore.OData.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OData.Edm;
using Microsoft.OData.UriParser;

namespace Microsoft.AspNetCore.OData.Tests.Extensions;

/// <summary>
/// A class to create HttpRequest for tests.
/// </summary>
public static class RequestFactory
{
    /// <summary>
    /// Creates the <see cref="HttpRequest"/> with OData configuration.
    /// </summary>
    /// <param name="method">The http method.</param>
    /// <param name="uri">The http request uri.</param>
    /// <param name="setupAction">The OData configuration.</param>
    /// <returns>The HttpRequest.</returns>
    public static HttpRequest Create(string method, string uri, Action<ODataOptions> setupAction = null)
    {
        HttpContext context = new DefaultHttpContext();
        var request = context.Request;

        IServiceCollection services = new ServiceCollection();
        if (setupAction != null)
        {
            services.Configure(setupAction);
        }

        context.RequestServices = services.BuildServiceProvider();

        request.Method = method;
        var requestUri = new Uri(uri);
        request.Scheme = requestUri.Scheme;
        request.Host = requestUri.IsDefaultPort ? new HostString(requestUri.Host) : new HostString(requestUri.Host, requestUri.Port);
        request.QueryString = new QueryString(requestUri.Query);
        request.Path = new PathString(requestUri.AbsolutePath);

        //request.Host = HostString.FromUriComponent(BaseAddress);
        //if (BaseAddress.IsDefaultPort)
        //{
        //    request.Host = new HostString(request.Host.Host);
        //}
        //var pathBase = PathString.FromUriComponent(BaseAddress);
        //if (pathBase.HasValue && pathBase.Value.EndsWith("/"))
        //{
        //    pathBase = new PathString(pathBase.Value[..^1]); // All but the last character.
        //}
        //request.PathBase = pathBase;

        return request;
    }
}