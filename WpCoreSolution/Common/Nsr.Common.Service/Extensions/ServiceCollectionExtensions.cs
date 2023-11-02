using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nsr.Common.Core;
using Nsr.Common.Core.Caching;
using Nsr.Common.Core.Localization;
using Nsr.Common.Data;
using Nsr.Common.Data.Repositories;
using Nsr.Common.Service.Configuration;
using Nsr.Common.Service.Localization;
using Nsr.Common.Services;
using System;

namespace Nsr.Common.Service.Extensions
{
    public static class ServiceCollectionExtensions
    {
        private static IServiceCollection AddDbContexts(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<NsrCommonDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("CommonConnection"),
                sqlServerOptionsAction: x =>
                {
                    x.MigrationsAssembly("Nsr.Common.Data");
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

            //services.AddEntityFrameworkProxies();

            return services;
        }

        //private static void Migrate(IApplicationBuilder app)
        //{
        //    using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
        //    {
        //        using (NsrCommonDbContext context = serviceScope.ServiceProvider.GetService<NsrCommonDbContext>())
        //        {
        //            context.Database.Migrate();

        //        }
        //    }
        //}     

        private static IServiceCollection AddServices(this IServiceCollection services)
        {

            // repositories
            services.AddScoped(typeof(ICommonBaseRepository<>), typeof(CommonBaseRepository<>));

            // services
            services.AddScoped<IWorkContext, WorkContext>();
            services.AddScoped<ICommonUnitOfWork, CommonUnitOfWork>();
            services.AddScoped<ICacheManager, PerRequestCacheManager>();

            services.AddScoped<ILanguageService, LanguageService>();
            services.AddScoped<ILocalizationService, LocalizationService>();
            services.AddScoped<ILocalizedEntityService, LocalizedEntityService>();

            services.AddScoped<ISettingService, SettingService>();
            //services.AddScoped(x =>
            //{
            //    return x.GetService<ISettingService>().LoadSetting<WebsiteSettings>();
            //});
            //services.AddScoped<ISettings>( sp =>
            //{
            //    var see = sp.GetService<ISettingService>().LoadSetting<LocalizationSettings>();
            //    return see;
            //});

            return services;
        }
   
        public static IServiceCollection AddNsrCommon(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContexts(configuration);
            services.AddServices();
            return services;             
        }

        public static void UseNsrCommon(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (NsrCommonDbContext context = serviceScope.ServiceProvider.GetService<NsrCommonDbContext>())
                {
                    context.Database.Migrate();

                }
            }

            ServiceLocator.Instance = app.ApplicationServices;

            //Migrate(app);
        }
    }
}
