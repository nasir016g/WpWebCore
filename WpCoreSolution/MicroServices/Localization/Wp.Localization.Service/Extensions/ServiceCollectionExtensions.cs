using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wp.Common;
using Wp.Localization.Data;
using Wp.Localization.Data.Repositories;
using Wp.Localization.Services;
using Wp.Services.Localization;

namespace Wp.Localization.Extensions
{
    public static class ServiceCollectionExtensions
    {
        private static IServiceCollection AddDbContexts(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<WpLocalizationDbContext>(options =>
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
                using (WpLocalizationDbContext context = serviceScope.ServiceProvider.GetService<WpLocalizationDbContext>())
                {
                    context.Database.Migrate();

                }


            }
        }     

        private static IServiceCollection AddServices(this IServiceCollection services)
        {

            // repositories
            services.AddScoped(typeof(ILocalizationBaseRepository<>), typeof(LocalizationBaseRepository<>));

            // services
            services.AddScoped<ILocalizationUnitOfWork, LocalizationUnitOfWork>();

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
                using (WpLocalizationDbContext context = serviceScope.ServiceProvider.GetService<WpLocalizationDbContext>())
                {
                    context.Database.Migrate();

                }

                var ls = serviceScope.ServiceProvider.GetService<ILanguageService>();
                var les = serviceScope.ServiceProvider.GetService<ILocalizedEntityService>();
                LocalizedUrlExtenstions.Configure(ls);
                LocalizationExtensions.Configure(ls, les);
            }
           
            Migrate(app);
        }
    }
}
