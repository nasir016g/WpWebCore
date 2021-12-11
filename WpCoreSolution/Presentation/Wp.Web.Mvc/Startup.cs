using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Nsr.Common.Core;
using Nsr.Common.Service.Extensions;
using Nsr.RestClient.Extensions;
using System;
using System.Reflection;
using Wp.Core.Security;
using Wp.Data;
using Wp.Web.Framework.Extensions;
using Wp.Web.Framework.Infrastructure.Mapper;
using Wp.Web.Framework.ViewEngines.Razor;
using Wp.Web.Mvc.Infrastructure.Mapper;
using Wp.Web.Mvc.Infrastructure.Routing;
using ServiceCollectionExtensions = Wp.Web.Framework.Extensions.ServiceCollectionExtensions;

namespace Wp.Web.Mvc
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddDistributedMemoryCache();
            services.AddNsrCommon(Configuration);


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

            services.AddWpAndCatalogDbContexts(Configuration);
            services.AddWp();
            services.AddRestClients(Configuration);
            services.AddLocalizationRestClients(Configuration);
            services.AddScoped<SlugRouteTransformer>();
           

            //add routing
            services.AddRouting(options =>
            {
                //add constraint key for language
                options.ConstraintMap["lang"] = typeof(LanguageParameterTransformer);
            });

            services.AddAutoMapper(typeof(Startup));
            AutoMapperConfiguration.Init();
            AutoMapperMvcConfiguration.Init();
            services.AddControllersWithViews()
                .AddFluentValidation(opt =>
                {
                    opt.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
                })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });

            
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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            ServiceLocator.Instance = app.ApplicationServices;
           Nsr.Common.Service.Extensions.ServiceCollectionExtensions.UseNsrCommon(app);
            app.UseSession();

            ServiceCollectionExtensions.Migrate(app);
            ServiceCollectionExtensions.AddLogger();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                var pattern = "{SeName}";
                using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    //var localizationSettings = serviceScope.ServiceProvider.GetRequiredService<LocalizationSettings>();
                    //if (localizationSettings.SeoFriendlyUrlsForLanguagesEnabled)
                    //{
                    //    var langservice = serviceScope.ServiceProvider.GetRequiredService<ILanguageService>();
                    //    var languages = langservice.GetAll().ToList();
                    //    pattern = "{language:lang=" + languages.First().UniqueSeoCode + "}/{SeName}";
                    //    //}
                    //}
                }


                endpoints.MapDynamicControllerRoute<SlugRouteTransformer>(pattern);
                endpoints.MapControllerRoute(name: "areas", pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
