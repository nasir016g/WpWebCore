using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nsr.Common.Core;
using Nsr.RestClient.RestClients.Localization;
using Refit;

namespace Nsr.RestClient.Extensions
{
    public static class ServiceCollectionExtensions
    {   
     
        public static IServiceCollection AddLocalizationRestClients(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IWorkContext, WorkContext>();
            services.AddScoped<ILocalizedEnitityHelperService, LocalizedEnitityHelperService>();

            string apiHostAndPort = configuration.GetSection("APIServiceLocations").GetValue<string>("LocalizationWebApi");
            var uri = new Uri($"http://{apiHostAndPort}");
            services.AddRefitClient<ILanguageWebApi>().ConfigureHttpClient(x => x.BaseAddress = uri);
            services.AddRefitClient<ILocalizationWebApi>().ConfigureHttpClient(x => x.BaseAddress = uri);
            services.AddRefitClient<ILocalizedEntityWebApi>().ConfigureHttpClient(x => x.BaseAddress = uri);
            //services.AddRefitClient<IEducationWebApi>().ConfigureHttpClient(x => x.BaseAddress = uri);
            //services.AddRefitClient<IExperienceWebApi>().ConfigureHttpClient(x => x.BaseAddress = uri);
            //services.AddRefitClient<ISkillWebApi>().ConfigureHttpClient(x => x.BaseAddress = uri);

            return services;
        }
    }
}
