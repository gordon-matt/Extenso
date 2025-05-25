using Autofac.Extensions.DependencyInjection;

namespace Demo.Extenso.AspNetCore.Mvc.OData;

public class Program
{
    public static void Main(string[] args) => CreateHostBuilder(args).Build().Run();

    public static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
        .UseServiceProviderFactory(new AutofacServiceProviderFactory());
}