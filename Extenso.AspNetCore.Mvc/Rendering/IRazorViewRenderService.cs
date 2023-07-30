using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;

namespace Extenso.AspNetCore.Mvc.Rendering
{
    // Based on: https://ppolyzos.com/2016/09/09/asp-net-core-render-view-to-string/
    public interface IRazorViewRenderService
    {
        Task<string> RenderToStringAsync(string viewName, object model = null, RouteData routeData = null, bool useActionContext = false);
    }

    public class RazorViewRenderService : IRazorViewRenderService
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IServiceProvider serviceProvider;
        private readonly IRazorViewEngine razorViewEngine;
        private readonly ITempDataProvider tempDataProvider;

        public RazorViewRenderService(
            IHttpContextAccessor httpContextAccessor,
            IServiceProvider serviceProvider,
            IRazorViewEngine razorViewEngine,
            ITempDataProvider tempDataProvider)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.serviceProvider = serviceProvider;
            this.razorViewEngine = razorViewEngine;
            this.tempDataProvider = tempDataProvider;
        }

        public async Task<string> RenderToStringAsync(string viewName, object model = null, RouteData routeData = null, bool useActionContext = false)
        {
            var actionContext = new ActionContext(
                httpContextAccessor == null ? new DefaultHttpContext { RequestServices = serviceProvider } : httpContextAccessor.HttpContext,
                routeData ?? new RouteData(),
                new ActionDescriptor());

            using (var stringWriter = new StringWriter())
            {
                ViewEngineResult viewResult;
                if (useActionContext)
                {
                    viewResult = razorViewEngine.FindView(actionContext, viewName, false);
                }
                else
                {
                    viewResult = razorViewEngine.GetView(viewName, viewName, false);
                }

                if (viewResult.View == null)
                {
                    throw new ArgumentNullException("View", $"{viewName} does not match any available view");
                }

                var viewDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                {
                    Model = model
                };

                var viewContext = new ViewContext(
                    actionContext,
                    viewResult.View,
                    viewDictionary,
                    new TempDataDictionary(actionContext.HttpContext, tempDataProvider),
                    stringWriter,
                    new HtmlHelperOptions()
                );

                await viewResult.View.RenderAsync(viewContext);
                return stringWriter.ToString();
            }
        }
    }
}