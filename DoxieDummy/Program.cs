using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace DoxieDummy;

/// <summary>
/// Represents the program.
/// </summary>
public class Program
{
    /// <summary>
    /// Performs the main operation.
    /// </summary>
    /// <param name="args">The args.</param>
    public static void Main(string[] args) => CreateHostBuilder(args).Build().Run();

    /// <summary>
    /// Creates the host builder.
    /// </summary>
    /// <param name="args">The args.</param>
    /// <returns>The host builder.</returns>
    public static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
}