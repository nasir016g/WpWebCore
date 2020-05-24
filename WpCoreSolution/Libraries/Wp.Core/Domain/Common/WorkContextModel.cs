
using Wp.Core.Domain.Localization;

namespace Wp.Core.Domain.Common
{
    public class WorkContextModel
    {        
        public WebsiteSettings WebSite { get; set; }
        public Language WorkingLanguage { get; set; }
    }
}
