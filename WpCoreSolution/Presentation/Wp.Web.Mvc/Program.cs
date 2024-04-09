using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Wp.Web.Mvc
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
                    webBuilder.ConfigureAppConfiguration(config =>
                    {
                        // Retrieve the connection string
                        IConfiguration settings = config.Build();
                        var appConfig = settings.GetSection("AppConfig");
                        string connectionString = appConfig.GetValue<string>("Connection");

                        // Load configuration from Azure App Configuration
                        config.AddAzureAppConfiguration(options =>
                        {
                            options.Connect(connectionString)
                            .ConfigureKeyVault(x =>
                            {
                                x.SetCredential(new DefaultAzureCredential());
                            });
                        });
                    });
                    webBuilder.UseStartup<Startup>();
                });
    }
}
