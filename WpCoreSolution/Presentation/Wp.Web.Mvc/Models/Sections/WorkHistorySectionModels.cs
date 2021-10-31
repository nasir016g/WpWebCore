using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Wp.Core.Domain.Sections;
using Wp.Services.WebPages;

namespace Wp.Web.Mvc.Models.Sections
{
    public class WorkHistorySectionModels
    {
        public WorkHistorySectionModels()
        {            
        }

        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int WebPageId { get; set; }

        public string Description { get; set; }
        public string ApplicationUserName { get; set; }
    }

    public class WorkHistorySectionReadOnlyModel : BaseReadOnlyModel
    {
        public string ApplicationUserName { get; set; }

        public WorkHistorySectionReadOnlyModel() {  }

        public WorkHistorySectionReadOnlyModel(Section section, IWebPageService webPageService, IHttpContextAccessor httpContextAccessor)
            : base(section, webPageService, httpContextAccessor) { }
    }
}