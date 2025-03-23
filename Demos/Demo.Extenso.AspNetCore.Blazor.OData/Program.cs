using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using OfficeOpenXml;

namespace Demo.Extenso.AspNetCore.Blazor.OData;

public class Program
{
    // TODO: There's gotta be a better way than this..
    internal static string ODataBaseUri = "https://localhost:58824/odata/";

    public static void Main(string[] args)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            })
            .UseServiceProviderFactory(new AutofacServiceProviderFactory());
}