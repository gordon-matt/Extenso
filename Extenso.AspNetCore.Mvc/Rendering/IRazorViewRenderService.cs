using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;

namespace Extenso.AspNetCore.Mvc.Rendering;

// Based on: https://ppolyzos.com/2016/09/09/asp-net-core-render-view-to-string/
/// <summary>
/// Defines the contract for the razor view render service.
/// </summary>
public interface IRazorViewRenderService
{
    /// <summary>
    /// Renders to the string.
    /// </summary>
    /// <param name="viewName">The view name.</param>
    /// <param name="model">The model.</param>
    /// <param name="routeData">The route data.</param>
    /// <param name="useActionContext">The use action context.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the string.</returns>
    Task<string> RenderToStringAsync(string viewName, object model = null, RouteData routeData = null, bool useActionContext = false);
}

/// <summary>
/// Represents the razor view render service.
/// </summary>
public class RazorViewRenderService : IRazorViewRenderService
{
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly IServiceProvider serviceProvider;
    private readonly IRazorViewEngine razorViewEngine;
    private readonly ITempDataProvider tempDataProvider;

    /// <summary>
    /// Initializes a new instance of the RazorViewRenderService class.
    /// </summary>
    /// <param name="httpContextAccessor">The HTTP context accessor.</param>
    /// <param name="serviceProvider">The service provider.</param>
    /// <param name="razorViewEngine">The razor view engine.</param>
    /// <param name="tempDataProvider">The temp data provider.</param>
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

    /// <summary>
    /// Renders to the string.
    /// </summary>
    /// <param name="viewName">The view name.</param>
    /// <param name="model">The model.</param>
    /// <param name="routeData">The route data.</param>
    /// <param name="useActionContext">The use action context.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the string.</returns>
    public async Task<string> RenderToStringAsync(string viewName, object model = null, RouteData routeData = null, bool useActionContext = false)
    {
        var actionContext = new ActionContext(
            httpContextAccessor == null ? new DefaultHttpContext { RequestServices = serviceProvider } : httpContextAccessor.HttpContext,
            routeData ?? new RouteData(),
            new ActionDescriptor());

        using var stringWriter = new StringWriter();
        var viewResult = useActionContext
            ? razorViewEngine.FindView(actionContext, viewName, false)
            : razorViewEngine.GetView(viewName, viewName, false);
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