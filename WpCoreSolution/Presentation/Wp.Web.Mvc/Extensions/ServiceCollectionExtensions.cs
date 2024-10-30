using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Nsr.Common.Service.Extensions;
using Nsr.RestClient.Extensions;
using Wp.Core.Security;
using Wp.Data;
using Wp.Web.Framework.Extensions;
using Wp.Web.Framework.Infrastructure.Mapper;
using Wp.Web.Framework.ViewEngines.Razor;
using Wp.Web.Mvc.Infrastructure.Mapper;
using Wp.Web.Mvc.Infrastructure.Routing;

namespace Wp.Web.Mvc.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddDistributedMemoryCache();
            services.AddNsrCommon(configuration);


            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(100);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            services.AddCors();
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDefaultIdentity<ApplicationUser>()
               .AddRoles<IdentityRole>()
               .AddEntityFrameworkStores<WpDbContext>();
            services.Configure<IdentityOptions>(options =>
            {
                //// Password settings.
                //options.Password.RequireDigit = true;
                //options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                //options.Password.RequiredUniqueChars = 1;

                //// Lockout settings.
                //options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                //options.Lockout.MaxFailedAccessAttempts = 5;
                //options.Lockout.AllowedForNewUsers = true;

                //// User settings.
                //options.User.AllowedUserNameCharacters =
                //"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                //options.User.RequireUniqueEmail = false;
            });

            services.AddWpAndCatalogDbContexts(configuration);
            services.AddWp();
            services.AddRestClients(configuration);
            services.AddActivityLogRestClients(configuration);
            services.AddScoped<SlugRouteTransformer>();


            //add routing
            services.AddRouting(options =>
            {
                //add constraint key for language
                options.ConstraintMap["lang"] = typeof(LanguageParameterTransformer);
            });

            services.AddAutoMapper(typeof(Program));
            AutoMapperConfiguration.Init();
            AutoMapperMvcConfiguration.Init();
           


            services.Configure<RazorViewEngineOptions>(o =>
            {
                o.ViewLocationExpanders.Add(new ThemeableViewLocationExpander());
                //o.ViewLocationFormats.Clear();
                //o.ViewLocationFormats.Add("/MyViewsFolder/{1}/{0}" + RazorViewEngine.ViewExtension);
                //o.ViewLocationFormats.Add("/MyViewsFolder/Shared/{0}" + RazorViewEngine.ViewExtension);
            });

            services.AddRazorPages();

            services.AddEasyCaching(option =>
            {
                // use memory cache with a simple way
                option.UseInMemory("default");
            });

            return services;
        }       
    }
}
