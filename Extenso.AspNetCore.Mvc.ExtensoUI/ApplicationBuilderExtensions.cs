using Extenso.AspNetCore.Mvc.ExtensoUI.Providers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Extenso.AspNetCore.Mvc.ExtensoUI
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseExtensoUI<TProvider>(this IApplicationBuilder app)
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