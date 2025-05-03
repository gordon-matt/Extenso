using Autofac;
using Demo.Extenso.AspNetCore.Mvc.OData.Data;
using Demo.Extenso.AspNetCore.Mvc.OData.Data.Entities;
using Demo.Extenso.AspNetCore.Mvc.OData.Infrastructure;
using Demo.Extenso.AspNetCore.Mvc.OData.Models;
using Demo.Extenso.AspNetCore.Mvc.OData.Services;
using Demo.Extenso.AspNetCore.OData.Models;
using Extenso.AspNetCore.Mvc.ExtensoUI;
using Extenso.AspNetCore.Mvc.ExtensoUI.Providers;
using Extenso.AspNetCore.OData;
using Extenso.Data.Entity;
using Extenso.Mapping;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;

namespace Demo.Extenso.AspNetCore.Mvc.OData;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseInMemoryDatabase("DemoDb"));

        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        // Add application services.
        services.AddTransient<IEmailSender, EmailSender>();

        services.AddControllersWithViews()
            //.AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore)
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

        services.AddRazorPages();

        #region ExtensoMapper Demo

        ExtensoMapper.Register<PersonModel, Person>(x => x.ToEntity());
        ExtensoMapper.Register<Person, PersonModel>(x => x.ToModel());

        #endregion ExtensoMapper Demo

        #region AutoMapper Demo

        //services.AddAutoMapper(cfg =>
        //{
        //    cfg.CreateMap<PersonModel, Person>();
        //    cfg.CreateMap<Person, PersonModel>();
        //});

        #endregion AutoMapper Demo
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
        }

        // For wwwroot directory
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
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            endpoints.MapRazorPages();
        });

        app.UseExtensoUI<Bootstrap3UIProvider>();
    }

    public void ConfigureContainer(ContainerBuilder builder)
    {
        builder.RegisterType<ApplicationDbContextFactory>().As<IDbContextFactory>().SingleInstance();

        builder.RegisterGeneric(typeof(EntityFrameworkRepository<>))
            .As(typeof(IRepository<>))
            .InstancePerLifetimeScope();

        #region ExtensoMapper Demo

        builder.RegisterGeneric(typeof(ExtensoMapperEntityFrameworkRepository<,>))
            .As(typeof(IMappedRepository<,>))
            .InstancePerLifetimeScope();

        #endregion ExtensoMapper Demo

        #region AutoMapper Demo

        //builder.RegisterGeneric(typeof(AutoMapperEntityFrameworkRepository<,>))
        //    .As(typeof(IMappedRepository<,>))
        //    .InstancePerLifetimeScope();

        #endregion AutoMapper Demo

        builder.RegisterType<ODataRegistrar>().As<IODataRegistrar>().SingleInstance();
    }
}