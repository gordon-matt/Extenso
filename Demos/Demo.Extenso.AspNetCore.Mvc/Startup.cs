using Autofac;
using Demo.Extenso.AspNetCore.Mvc.Data;
using Demo.Extenso.AspNetCore.Mvc.Data.Entities;
using Demo.Extenso.AspNetCore.Mvc.Models;
using Extenso.AspNetCore.Mvc.ExtensoUI;
using Extenso.AspNetCore.Mvc.ExtensoUI.Providers;
using Extenso.Data.Entity;
using Extenso.Mapping;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Demo.Extenso.AspNetCore.Mvc;

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

        services.Configure<CookiePolicyOptions>(options =>
        {
            // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            options.CheckConsentNeeded = context => true;
            options.MinimumSameSitePolicy = SameSiteMode.None;
        });

        services.AddControllersWithViews();

        services.AddRazorPages();

        #region ExtensoMapper Demo

        ExtensoMapper.Register<PersonModel, Person>(x => x.ToEntity());
        ExtensoMapper.Register<Person, PersonModel>(x => x.ToModel());

        #endregion

        #region AutoMapper Demo

        //services.AddAutoMapper(cfg =>
        //{
        //    cfg.CreateMap<PersonModel, Person>();
        //    cfg.CreateMap<Person, PersonModel>();
        //});

        #endregion
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //app.UseHsts();
        }

        //app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseCookiePolicy();

        app.UseRouting();

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

        #region ExtensoMapper Demo

        builder.RegisterGeneric(typeof(ExtensoMapperEntityFrameworkRepository<,>))
            .As(typeof(IMappedRepository<,>))
            .InstancePerLifetimeScope();

        #endregion

        #region AutoMapper Demo

        //builder.RegisterGeneric(typeof(AutoMapperEntityFrameworkRepository<,>))
        //    .As(typeof(IMappedRepository<,>))
        //    .InstancePerLifetimeScope();

        #endregion
    }
}