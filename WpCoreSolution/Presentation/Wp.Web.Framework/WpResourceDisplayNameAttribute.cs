using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using Wp.Core;
using Wp.Services.Localization;

namespace Wp.Web.Framework
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
                var langId = ServiceLocator.Instance.GetService<IWorkContext>().Current.WorkingLanguage.Id;
                _resourceValue = ServiceLocator.Instance.GetService<ILocalizationService>().GetResource(ResourceKey, langId);
                if (string.IsNullOrEmpty(_resourceValue))
                {
                    return ResourceKey;
                }
                return _resourceValue;
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
