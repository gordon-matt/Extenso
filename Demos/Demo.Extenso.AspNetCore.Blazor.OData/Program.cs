using Autofac.Extensions.DependencyInjection;
using OfficeOpenXml;

namespace Demo.Extenso.AspNetCore.Blazor.OData;

public class Program
{
    public static void Main(string[] args)
    {
        ExcelPackage.License.SetNonCommercialPersonal("Extenso");
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
        .UseServiceProviderFactory(new AutofacServiceProviderFactory());
}