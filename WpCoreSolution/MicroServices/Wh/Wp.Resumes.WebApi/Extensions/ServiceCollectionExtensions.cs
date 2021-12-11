using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nsr.Common.Core;
using Nsr.RestClient;
using Nsr.RestClient.RestClients.Localization;
using Refit;
using System;
using Wp.Wh.Data;
using Wp.Wh.Data.Repositories;
using Wp.Wh.Services;
using Wp.Wh.Services.ExportImport;

namespace Wp.Wh.WebApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDbContexts(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddEntityFrameworkSqlServer();
            services.AddDbContext<WpWhDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                sqlServerOptionsAction: x =>
                {
                    x.MigrationsAssembly("Wp.Wh.Data");
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
                using (WpWhDbContext context = serviceScope.ServiceProvider.GetService<WpWhDbContext>())
                {
                    context.Database.Migrate();

                }
            }
        }     

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));            
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IEducationService, EducationService>();
            services.AddScoped<IExperienceService, ExperienceService>();
            services.AddScoped<IResumeService, ResumeService>();
            services.AddScoped<ISkillService, SkillService>();
            services.AddScoped<IImportManager, ImportManager>();
            services.AddScoped<IExportManager, ExportManager>();
            services.AddScoped<IPdfService, PdfService>();

            return services;
        }
       
    }
}
