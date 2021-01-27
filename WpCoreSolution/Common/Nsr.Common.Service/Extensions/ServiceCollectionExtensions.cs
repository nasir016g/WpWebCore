using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nsr.Common.Core.Localization;
using Nsr.Common.Data;
using Nsr.Common.Data.Repositories;
using Nsr.Common.Services;
using System;

namespace Wp.Localization.Extensions
{
    public static class ServiceCollectionExtensions
    {
        private static IServiceCollection AddDbContexts(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<WpCommonDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("LocalizationConnection"),
                sqlServerOptionsAction: x =>
                {
                    x.MigrationsAssembly("Wp.Localization.Data");
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

        private static void Migrate(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (WpCommonDbContext context = serviceScope.ServiceProvider.GetService<WpCommonDbContext>())
                {
                    context.Database.Migrate();

                }


            }
        }     

        private static IServiceCollection AddServices(this IServiceCollection services)
        {

            // repositories
            services.AddScoped(typeof(ICommonBaseRepository<>), typeof(CommonBaseRepository<>));

            // services
            services.AddScoped<ICommonUnitOfWork, CommonUnitOfWork>();

            services.AddScoped<ILanguageService, LanguageService>();
            services.AddScoped<ILocalizationService, LocalizationService>();
            services.AddScoped<ILocalizedEntityService, LocalizedEntityService>();

            return services;
        }
   
        public static IServiceCollection AddLocalization(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContexts(configuration);
            services.AddServices();
            return services;             
        }

        public static void UseLocalization(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (WpCommonDbContext context = serviceScope.ServiceProvider.GetService<WpCommonDbContext>())
                {
                    context.Database.Migrate();

                }

            //    var ls = serviceScope.ServiceProvider.GetService<ILanguageService>();
            //    var les = serviceScope.ServiceProvider.GetService<ILocalizedEntityService>();
            //    LocalizedUrlExtenstions.Configure(ls);
            //    LocalizationExtensions.Configure(ls, les);
            }


           //var ls = app.ApplicationServices.GetService<ILanguageService>();
           // var les = app.ApplicationServices.GetService<ILocalizedEntityService>();
           // LocalizedUrlExtenstions.Configure(ls);
           // LocalizationExtensions.Configure(ls, les);

            ServiceLocator.Instance = app.ApplicationServices;

            Migrate(app);
        }
    }
}
