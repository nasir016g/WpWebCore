using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nsr.RestClient.RestClients.ActivityLogs;
using Refit;

namespace Nsr.RestClient.Extensions
{
    public static class ServiceCollectionExtensions
    {    
      

        public static IServiceCollection AddActivityLogRestClients(this IServiceCollection services, IConfiguration configuration)
        {
            string apiHostAndPort = configuration.GetSection("APIServiceLocations").GetValue<string>("ActivityLogWebApi");
            var uri = new Uri($"http://{apiHostAndPort}");

            services.AddRefitClient<IActivityLogWebApi>().ConfigureHttpClient(x => x.BaseAddress = uri);
            return services;
        }
    }
}
