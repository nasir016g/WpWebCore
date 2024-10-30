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
            string activityLogUrl = configuration.GetSection("APIServiceLocations").GetValue<string>("ActivityLogUrl");
            //var activityLogUrl = configuration.GetValue<string>("ActivityLogUrl"); // azure app configuration
            var uri = new Uri(activityLogUrl);

            services.AddRefitClient<IActivityLogWebApi>().ConfigureHttpClient(x => x.BaseAddress = uri);
            return services;
        }
    }
}
