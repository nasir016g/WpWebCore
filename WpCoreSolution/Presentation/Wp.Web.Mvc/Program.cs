
using Azure.Identity;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Nsr.Common.Core;
using NuGet.Packaging;
using Wp.Web.Mvc.Extensions;
using Wp.Web.Mvc.Infrastructure.Routing;
using Xceed.Document.NET;
using FrameWorkExtenstions = Wp.Web.Framework.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
               //.AddFluentValidation(opt =>
               //{
               //    opt.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
               //})
               .AddNewtonsoftJson(options =>
               {
                   options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                   options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
               });
// Retrieve the connection string
var appConfig = builder.Configuration.GetSection("AppConfig");
string connectionString = appConfig.GetValue<string>("Connection");

// Load configuration from Azure App Configuration
builder.Configuration.AddAzureAppConfiguration(options =>
{
    options.Connect(connectionString)
    .ConfigureKeyVault(x =>
    {
        x.SetCredential(new DefaultAzureCredential());
    });
});


IConfiguration configuration = builder.Configuration;
builder.Services.ConfigureServices(configuration);

var app = builder.Build();

ServiceLocator.Instance = app.Services;
Nsr.Common.Service.Extensions.ServiceCollectionExtensions.UseNsrCommon(app);
app.UseSession();

FrameWorkExtenstions.ServiceCollectionExtensions.Migrate(app);
FrameWorkExtenstions.ServiceCollectionExtensions.AddLogger();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCookiePolicy();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    var pattern = "{SeName}";

    endpoints.MapDynamicControllerRoute<SlugRouteTransformer>(pattern);
    endpoints.MapControllerRoute(name: "areas", pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
    endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
    endpoints.MapRazorPages();
});

app.Run();








//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Azure.Identity;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Logging;

//namespace Wp.Web.Mvc
//{
//    public class Program
//    {
//        public static void Main(string[] args)
//        {
//            CreateHostBuilder(args).Build().Run();
//        }

//        public static IHostBuilder CreateHostBuilder(string[] args) =>
//            Host.CreateDefaultBuilder(args)
//                .ConfigureWebHostDefaults(webBuilder =>
//                {
//                    webBuilder.ConfigureAppConfiguration(config =>
//                    {
//                        // Retrieve the connection string
//                        IConfiguration settings = config.Build();
//                        var appConfig = settings.GetSection("AppConfig");
//                        string connectionString = appConfig.GetValue<string>("Connection");

//                        // Load configuration from Azure App Configuration
//                        config.AddAzureAppConfiguration(options =>
//                        {
//                            options.Connect(connectionString)
//                            .ConfigureKeyVault(x =>
//                            {
//                                x.SetCredential(new DefaultAzureCredential());
//                            });
//                        });
//                    });
//                    webBuilder.UseStartup<Startup>();
//                });
//    }
//}
