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

namespace Wp.Localization.ManagementApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWpLocalizationDbContexts(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddEntityFrameworkSqlServer();
            services.AddDbContext<WpLocalizationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
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

        public static void Migrate(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (WpLocalizationDbContext context = serviceScope.ServiceProvider.GetService<WpLocalizationDbContext>())
                {
                    context.Database.Migrate();

                }
            }
        }     

        public static IServiceCollection AddWpLocalization(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // repositories
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            //services.AddScoped<ISectionRepository, SectionRepository>();

            // services
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            //services.AddScoped(typeof(IEntityService<>), typeof(EntityService<>));

            services.AddScoped<ILanguageService, LanguageService>();
            services.AddScoped<ILocalizationService, LocalizationService>();
            services.AddScoped<ILocalizedEntityService, LocalizedEntityService>();
            //services.AddScoped(x =>
            //{
            //    return x.GetService<ISettingService>().LoadSetting<LocalizationSettings>(); 
            //});

            return services;
        }
    }
}
