using Microsoft.EntityFrameworkCore;
using Nsr.Common.Core;
using Nsr.Localization.Web.Api.Data;
using Nsr.Localization.Web.Api.Data.Repositories;
using Nsr.Localization.Web.Api.Service;
using Nsr.Localization.Web.Api.Services;

namespace Nsr.Localization.Web.Api
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDbContexts(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddEntityFrameworkSqlServer();
            services.AddDbContext<LocalizationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                sqlServerOptionsAction: x =>
                {
                    //x.MigrationsAssembly("Wp.Wh.Data");
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
                using (LocalizationDbContext context = serviceScope.ServiceProvider.GetService<LocalizationDbContext>())
                {
                    context.Database.Migrate();

                }
            }
        }     

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            
            services.AddScoped(typeof(ILocalizationBaseRepository<>), typeof(LocalizationBaseRepository<>));            
            services.AddScoped<ILocalizationUnitOfWork, LocalizationUnitOfWork>();
            services.AddScoped<ILanguageService, LanguageService>();
            services.AddScoped<ILocalizationService, LocalizationService>();
            services.AddScoped<ILocalizedEntityService, LocalizedEntityService>();
            services.AddScoped<ILocalizationInstallationService, LocalizationInstallationService>();

            return services;
        }
    }
}
