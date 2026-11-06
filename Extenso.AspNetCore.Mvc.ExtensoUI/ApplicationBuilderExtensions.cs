using Extenso.AspNetCore.Mvc.ExtensoUI.Providers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Extenso.AspNetCore.Mvc.ExtensoUI;

/// <summary>
/// Provides extension methods for application builders.
/// </summary>
public static class ApplicationBuilderExtensions
{
    extension(IApplicationBuilder app)
    {
        /// <summary>
        /// Uses the extenso UI.
        /// </summary>
        /// <typeparam name="TProvider">The type of the provider.</typeparam>
        public void UseExtensoUI<TProvider>()
            where TProvider : BaseUIProvider, new()
        {
            var actionContext = new ActionContext(
                new DefaultHttpContext { RequestServices = app.ApplicationServices },
                new RouteData(),
                new ActionDescriptor());

            var urlHelperFactory = app.ApplicationServices.GetRequiredService<IUrlHelperFactory>();
            Internal.UrlHelper = urlHelperFactory.GetUrlHelper(actionContext);

            var provider = new TProvider();
            ExtensoUISettings.Init(provider, provider);
        }
    }
}