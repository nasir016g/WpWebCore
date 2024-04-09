using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nsr.Common.Core;
using Nsr.Work.Web.Data;
using Nsr.Work.Web.Data.Repositories;
using Nsr.Work.Web.Services;
using Nsr.Work.Web.Services.ExportImport;
using System;

namespace Nsr.Work.Web.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDbContexts(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddEntityFrameworkSqlServer();
            services.AddDbContext<WpWhDbContext>(options =>
            {
                //var connString = configuration.GetConnectionString("DefaultConnection");
                //var connString = configuration.GetValue<string>("NsrConnString");
                var connString = configuration.GetValue<string>("KV_Dev_Nsr_ConnString");


                options.UseSqlServer(connString,
                sqlServerOptionsAction: x =>
                {
                    x.MigrationsAssembly("Nsr.Work.Web");
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
