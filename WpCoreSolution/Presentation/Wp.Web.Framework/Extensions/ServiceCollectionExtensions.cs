using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nsr.Common.Core;
using Nsr.Common.Core.Caching;
using Nsr.Common.Service.Configuration;
using Nsr.RestClient.RestClients;
using Refit;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using Wp.Core;
using Wp.Data;
using Wp.Data.Repositories;
using Wp.Service.Navigation;
using Wp.Service.Security;
using Wp.Service.Tenants;
using Wp.Services.Common;
using Wp.Services.Events;
using Wp.Services.Installation;
using Wp.Services.Sections;
using Wp.Services.Seo;
using Wp.Services.WebPages;
using Wp.Services.Websites;

namespace Wp.Web.Framework.Extensions
{
    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection AddWpAndCatalogDbContexts(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddEntityFrameworkSqlServer();
            services.AddDbContext<WpDbContext>(options =>
            {
                var connString = configuration.GetConnectionString("DefaultConnection");
                //var connString = configuration.GetValue<string>("NsrConnString"); 
                //var connString = configuration.GetValue<string>("KV_Dev_Nsr_ConnString"); 

                options.UseSqlServer(connString,
                sqlServerOptionsAction: x =>
                {
                    x.MigrationsAssembly("Wp.Data");
                    x.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                });
                //options
                ////.UseLazyLoadingProxies(useLazyLoadingProxies: true)
                //.UseSqlite(configuration.GetConnectionString("DefaultConnection"),
                //sqliteOptionsAction: x =>
                //{
                //    x.MigrationsAssembly("Wp.Data");
                //});
            });
            
            services.AddDbContext<TenantDbContext>(options =>
            {
                var connString = configuration.GetConnectionString("DefaultConnection");
                //var connString = configuration.GetValue<string>("NsrConnString");
                options.UseSqlServer(connString,
                sqlServerOptionsAction: x =>
                {
                    x.MigrationsAssembly("Wp.Data");
                    x.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                });
                //options.UseSqlite(configuration.GetConnectionString("CatalogConnection"),
                //sqliteOptionsAction: x =>
                //{
                //    x.MigrationsAssembly("Wp.Data");
                //});
            });

            services.AddEntityFrameworkProxies();

            services.AddScoped<ITenantService, TenantService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            return services;
        }

        public static void Migrate(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (TenantDbContext tContext = serviceScope.ServiceProvider.GetService<TenantDbContext>())
                {
                    tContext.Database.Migrate();
                    var tenantService = serviceScope.ServiceProvider.GetService<ITenantService>();
                    tenantService.InstallTenants();


                    // * I have commented out the following code snippet since i can't utilize multiple databases on my Azure free subscription *
                    using (WpDbContext context = serviceScope.ServiceProvider.GetService<WpDbContext>())
                    {
                        context.Database.Migrate();

                        //tenantService.InstallTenants();
                        var tenants = tenantService.GetAll();
                        foreach (var t in tenants)
                        {
                            WpDbContext wpContext = new WpDbContext(new DbContextOptions<WpDbContext>(), t.ConnectionString);
                            //wpContext.Database.Migrate();
                        }
                    }

                }
            }
        }

        public static IServiceCollection AddWp(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // repositories
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped(typeof(ITenantsBaseRepository), typeof(TenantsBaseRepository));

            // services            
            services.AddScoped<ITenantService, TenantService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IWebsiteService, WebsiteService>();
            services.AddScoped<IWebPageService, WebPageService>();
            services.AddScoped<ISectionService, SectionService>();
            services.AddScoped<ISettingService, SettingService>();
            services.AddScoped<IThemeService, ThemeService>();

            //// export/import services
            //services.AddScoped<IImportManager, ImportManager>();

            services.AddScoped<INavigationService, NavigationService>();
            services.AddScoped<IUrlRecordService, UrlRecordService>();
            services.AddScoped<ICacheManager, PerRequestCacheManager>();
            services.AddScoped<IStaticCacheManager, EasyMemoryCacheManager>();
            services.AddScoped<IInstallationService, CodeFirstInstallationService>();
            services.AddScoped<IClaimProvider, StandardClaimProvider>();

            services.AddScoped<IEventPublisher, EventPublisher>();
            services.AddScoped<IWebHelper, WebHelper>();

            return services;
        }

        public static IServiceCollection AddRestClients(this IServiceCollection services, IConfiguration configuration)
        {
            // Retrieve the base URI for the web APIs from configuration
            string profileUrl = configuration.GetSection("APIServiceLocations").GetValue<string>("ProfileUrl");
            //var profileUrl = configuration.GetValue<string>("ProfileUrl"); // azure app configuration

            // Construct the complete URI with the retrieved host and port
            var uri = new Uri(profileUrl);

            // Add Refit clients, configuring its base addresses
            services.AddRefitClient<IResumesWebApi>().ConfigureHttpClient(x => x.BaseAddress = uri);
            services.AddRefitClient<IEducationWebApi>().ConfigureHttpClient(x => x.BaseAddress = uri);
            services.AddRefitClient<IExperienceWebApi>().ConfigureHttpClient(x => x.BaseAddress = uri);
            services.AddRefitClient<ISkillWebApi>().ConfigureHttpClient(x => x.BaseAddress = uri);            

            return services;
        }


        public static void AddLogger()
        {
            // Configure Serilog logger
            Log.Logger = new LoggerConfiguration()
              // Set minimum log level to Debug
              .MinimumLevel.Debug()
              // Override minimum log level for specific namespaces
              .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
              .MinimumLevel.Override("System", LogEventLevel.Warning)
              .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
              // Enrich log events with contextual information
              .Enrich.FromLogContext()
              // Write log events to the console with a specified output template and theme
              .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Literate)
              // Write log events to a file with specified options
              .WriteTo.File(@"C:\home\LogFiles\Application\myapp.txt", fileSizeLimitBytes: 1_000_000, rollOnFileSizeLimit: true, shared: true, flushToDiskInterval: TimeSpan.FromSeconds(1))
              .CreateLogger();
        }
    }
}
