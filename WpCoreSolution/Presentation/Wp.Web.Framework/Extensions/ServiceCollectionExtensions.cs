﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using Wp.Core;
using Wp.Core.Caching;
using Wp.Core.Domain.Common;
using Wp.Core.Interfaces.Repositories;
using Wp.Data;
using Wp.Data.Repositories;
using Wp.Data.Repositories.Frontend;
using Wp.Service.Security;
using Wp.Service.Tenants;
using Wp.Services.Career;
using Wp.Services.Configuration;
using Wp.Services.Events;
using Wp.Services.Expenses;
using Wp.Services.ExportImport;
using Wp.Services.Frontend;
using Wp.Services.Installation;
using Wp.Services.Localization;
using Wp.Services.Presentation;
using Wp.Services.Sections;
using Wp.Services.Seo;
using Wp.Services.WebPages;

namespace Wp.Web.Framework.Extensions
{
    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection AddWpAndCatalogDbContexts(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddEntityFrameworkSqlServer();
            services.AddDbContext<WpDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
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
            services.AddDbContext<FrontendDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("FrontendConnection"),
                sqlServerOptionsAction: x =>
                {
                    x.MigrationsAssembly("Wp.Data");
                    x.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                });
                //options
                //.UseSqlite(configuration.GetConnectionString("FrontendConnection"),
                //sqliteOptionsAction: x =>
                //{
                //    x.MigrationsAssembly("Wp.Data");
                //});
            });
            services.AddDbContext<TenantDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("CatalogConnection"),
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
                    using (WpDbContext context = serviceScope.ServiceProvider.GetService<WpDbContext>())
                    {
                        context.Database.Migrate();

                        tenantService.InstallTenants();
                        var tenants = tenantService.GetAll();
                        foreach (var t in tenants)
                        {
                            WpDbContext wpContext = new WpDbContext(new DbContextOptions<WpDbContext>(), t.ConnectionString);
                            wpContext.Database.Migrate();

                            FrontendDbContext pContext = new FrontendDbContext(new DbContextOptions<FrontendDbContext>(), t.ConnectionString);
                            pContext.Database.Migrate();
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
            services.AddScoped(typeof(IExpenseRepository), typeof(ExpenseRepository));
            services.AddScoped(typeof(IExpenseCategoryRepository), typeof(ExpenseCategoryRepository));
            //services.AddScoped<IWebPageRepository, WebPageRepository>();
            //services.AddScoped<IWebPageRoleRepository, WebPageRoleRepository>();
            //services.AddScoped<ISectionRepository, SectionRepository>();

            // services
            services.AddScoped<ITenantService, TenantService>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            //services.AddScoped(typeof(IEntityService<>), typeof(EntityService<>));

            services.AddScoped<IWebPageService, WebPageService>();
            services.AddScoped<ISectionService, SectionService>();
            services.AddScoped<ISettingService, SettingService>();
            services.AddScoped<ILanguageService, LanguageService>();
            //services.AddScoped<IIdentityService, IIdentityService>();

            // career
            services.AddScoped<IResumeService, ResumeService>();
            services.AddScoped<IEducationService, EducationService>();
            services.AddScoped<IExperienceService, ExperienceService>();
            services.AddScoped<ISkillService, SkillService>();

            // expense services
            services.AddScoped<IExpenseService, ExpenseService>();
            services.AddScoped<IExpenseAccountService, ExpenseAccountService>();
            services.AddScoped<IExpenseCategoryService, ExpenseCategoryService>();
            services.AddScoped<IExpenseTagService, ExpenseTagService>();

            // export/import services
            services.AddScoped<IImportManager, ImportManager>();
            services.AddScoped<IImportExcelService, ImportExcelService>();

            services.AddScoped<IUrlRecordService, UrlRecordService>();
            services.AddScoped<ILocalizedEntityService, LocalizedEntityService>();
            services.AddScoped<ICacheManager, PerRequestCacheManager>();
            services.AddScoped<IStaticCacheManager, EasyMemoryCacheManager>();
            services.AddScoped<IInstallationService, CodeFirstInstallationService>();
            services.AddScoped<IClaimProvider, StandardClaimProvider>();
            services.AddScoped(x =>
            {
                return x.GetService<ISettingService>().LoadSetting<WebsiteSettings>();
            });

            //services.AddSingleton<IWorkContext, WorkContext>();
            services.AddScoped<IWorkContext, WorkContext>();
            return services;
        }        

        public static IServiceCollection AddFrontend(this IServiceCollection services)
        {
            services.AddScoped(typeof(IFrontendBaseRepository<>), typeof(FrontendBaseRepository<>));
            services.AddScoped<IFrontentWebPageService, FrontendWebPageService>();


            //event consumers
            services.AddScoped<IEventPublisher, EventPublisher>();

           var interfaces = typeof(FrontendEventConsumer).FindInterfaces((type, criteria) =>
            {
                var isMatch = type.IsGenericType && ((Type)criteria).IsAssignableFrom(type.GetGenericTypeDefinition());
                return isMatch;
            }, typeof(IConsumer<>));

            foreach (var iface in interfaces)
            {
                services.AddScoped(iface, typeof(FrontendEventConsumer));
            }

           // services.AddScoped()
           // services.AddScoped(interfaces), typeof(PresentationEventConsumer));


            return services;
        }
        public static void AddLogger()
        {
            Log.Logger = new LoggerConfiguration()
              .MinimumLevel.Debug()
              .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
              .MinimumLevel.Override("System", LogEventLevel.Warning)
              .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
              .Enrich.FromLogContext()
              .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Literate)
              .WriteTo.File(@"C:\home\LogFiles\Application\myapp.txt", fileSizeLimitBytes: 1_000_000, rollOnFileSizeLimit: true, shared: true, flushToDiskInterval: TimeSpan.FromSeconds(1))
              .CreateLogger();
        }
    }
}
