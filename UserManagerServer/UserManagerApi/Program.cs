using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Formatting.Json;

namespace UserManagerApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();

                    webBuilder.UseSerilog((hostingContext, loggerConfiguration) =>
                        loggerConfiguration
                            .ReadFrom.Configuration(hostingContext.Configuration)
                            .WriteTo.File(
                                formatter: new JsonFormatter(),
                                path: $@"{Directory.GetCurrentDirectory()}\logs\user-manager.json",
                                shared: false));
                });
    }
}
