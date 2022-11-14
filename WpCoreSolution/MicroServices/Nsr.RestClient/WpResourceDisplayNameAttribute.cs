using Microsoft.Extensions.DependencyInjection;
using Nsr.Common.Core;
using Nsr.Common.Service.Localization;
using System.ComponentModel;

//namespace Wp.Web.Framework
namespace Nsr.RestClient
{
    public class WpResourceDisplayNameAttribute : DisplayNameAttribute
    {
        private string _resourceValue = string.Empty;

        public WpResourceDisplayNameAttribute(string resourceKey)
            : base(resourceKey)
        {
            ResourceKey = resourceKey;
        }

        public string ResourceKey { get; set; }

        public override string DisplayName
        {
            get
            {
                using(var serviceScope = ServiceLocator.GetScope())
                {
                    var langId = serviceScope.ServiceProvider.GetService<IWorkContext>().Current.WorkingLanguageId;
                    _resourceValue = serviceScope.ServiceProvider.GetService<ILocalizationService>().GetResource(ResourceKey, langId);
                    if (string.IsNullOrEmpty(_resourceValue))
                    {
                        return ResourceKey;
                    }
                    return _resourceValue;
                }

               
            }
        }
        /// <summary>
        /// Gets name of the attribute
        /// </summary>
        public string Name
        {
            get { return nameof(WpResourceDisplayNameAttribute); }
        }
    }
}
