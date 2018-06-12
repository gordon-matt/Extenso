﻿using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Demo.AspNetCore.Mvc.ExtensoUI.KendoBootstrap3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}