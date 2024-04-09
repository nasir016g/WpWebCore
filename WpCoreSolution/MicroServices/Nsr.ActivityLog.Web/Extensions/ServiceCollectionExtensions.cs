using Microsoft.EntityFrameworkCore;
using Nsr.ActivityLogs.Web.Data;
using Nsr.ActivityLogs.Web.Data.Repositories;
using Nsr.ActivityLogs.Web.Service;
using Nsr.ActivityLogs.Web.Service.Abstract;
using Nsr.ActivityLogs.Web.Service.Installation;

namespace Nsr.ActivityLogs.Web.Extensions
{
    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection AddDbContexts(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddEntityFrameworkSqlServer();
            services.AddDbContext<ActivityLogDbContext>(options =>
            {
                //var connString = configuration.GetConnectionString("DefaultConnection");
                var connString = configuration.GetValue<string>("NsrConnString");
                options.UseSqlServer(connString,
                sqlServerOptionsAction: x =>
                {
                    //x.MigrationsAssembly("Nsr.Work.Web");
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
                using (ActivityLogDbContext context = serviceScope.ServiceProvider.GetService<ActivityLogDbContext>())
                {
                    context.Database.Migrate();

                }
            }
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IActivityLogBaseRepository<>), typeof(ActivityLogBaseRepository<>));
            services.AddScoped<IActivityLogUnitOfWork, ActivityLogUnitOfWork>();
            services.AddScoped<IActivityLogInstallationService, ActivityLogInstallationService>();
            services.AddScoped<IActivityLogService, ActivityLogService>();
            services.AddScoped<IActivityLogTypeService, ActivityLogTypeService>();
           

            return services;
        }
    }
}
