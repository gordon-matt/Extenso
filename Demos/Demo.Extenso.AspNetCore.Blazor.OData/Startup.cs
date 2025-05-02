using Autofac;
using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using Demo.Extenso.AspNetCore.Blazor.OData.Areas.Identity;
using Demo.Extenso.AspNetCore.Blazor.OData.Data;
using Demo.Extenso.AspNetCore.Blazor.OData.Data.Entities;
using Demo.Extenso.AspNetCore.Blazor.OData.Infrastructure;
using Demo.Extenso.AspNetCore.Blazor.OData.Services;
using Extenso.AspNetCore.OData;
using Extenso.Data.Entity;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Radzen;

namespace Demo.Extenso.AspNetCore.Blazor.OData;

public class Startup
{
    public Startup(IWebHostEnvironment currentEnvironment, IConfiguration configuration)
    {
        CurrentEnvironment = currentEnvironment;
        Configuration = configuration;
    }

    private IWebHostEnvironment CurrentEnvironment { get; set; }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                Configuration.GetConnectionString("DefaultConnection")));

        services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddRazorPages()
            .AddNewtonsoftJson()
            .AddOData((options, serviceProvider) =>
            {
                options.Select().Expand().Filter().OrderBy().SetMaxTop(null).Count();

                var registrars = serviceProvider.GetRequiredService<IEnumerable<IODataRegistrar>>();
                foreach (var registrar in registrars)
                {
                    registrar.Register(options);
                }
            });

        services.AddServerSideBlazor();
        services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();
        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddBlazorise(options =>
        {
            //options.ChangeTextOnKeyPress = true; // optional
        })
        .AddBootstrapProviders()
        .AddFontAwesomeIcons();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        // Use odata route debug, /$odata
        app.UseODataRouteDebug();

        // If you want to use /$openapi, enable the middleware.
        //app.UseODataOpenApi();

        // Add OData /$query middleware
        app.UseODataQueryRequest();

        // Add the OData Batch middleware to support OData $Batch
        //app.UseODataBatching();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapBlazorHub();
            endpoints.MapFallbackToPage("/_Host");
        });
    }

    public void ConfigureContainer(ContainerBuilder builder)
    {
        builder.RegisterType<ApplicationDbContextFactory>().As<IDbContextFactory>().SingleInstance();

        builder.RegisterGeneric(typeof(EntityFrameworkRepository<>))
            .As(typeof(IRepository<>))
            .InstancePerLifetimeScope();

        builder.RegisterType<ODataRegistrar>().As<IODataRegistrar>().SingleInstance();

        // Radzen
        builder.RegisterType<DialogService>().AsSelf().InstancePerLifetimeScope();
        builder.RegisterType<NotificationService>().AsSelf().InstancePerLifetimeScope();

        // Services
        builder.RegisterType<PersonODataService>().As<IGenericODataService<Person, int>>().SingleInstance();
        //builder.RegisterGeneric(typeof(GenericODataService<,>))
        //    .As(typeof(IGenericODataService<,>))
        //    .InstancePerLifetimeScope();
    }
}