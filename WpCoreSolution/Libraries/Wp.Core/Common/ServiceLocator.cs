using Microsoft.AspNetCore.Http;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wp.Core
{
    public static class ServiceLocator
    {
        public static IServiceProvider Instance { get; set; }

        public static IServiceScope GetScope(IServiceProvider instance = null)
        {
            var provider = instance ?? Instance;
            return provider?
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope();
        }

        public static IEnumerable<T> ResolveAll<T>()
        {
            return (IEnumerable<T>)Instance.GetRequiredService<IServiceScopeFactory>().CreateScope().ServiceProvider.GetServices(typeof(T));
            //using (var serviceScope = Instance.GetRequiredService<IServiceScopeFactory>().CreateScope())
            //{
            //  return (IEnumerable<T>)serviceScope.ServiceProvider.GetServices(typeof(T));
            //}
        }
    }
}
