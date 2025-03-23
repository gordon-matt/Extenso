using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Extenso.AspNetCore.Mvc.Tests;

internal class Startup
{
    public void ConfigureServices(IServiceCollection services) => services.AddControllersWithViews();//services.AddHttpContextAccessor();

    public void Configure(IApplicationBuilder app)
    {
    }
}