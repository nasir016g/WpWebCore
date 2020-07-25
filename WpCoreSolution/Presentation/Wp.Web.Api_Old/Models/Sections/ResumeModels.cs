using Microsoft.AspNetCore.Http;
using Wp.Core.Domain.Sections;
using Wp.Services.WebPages;

namespace Wp.Web.Models.Sections
{
    public class ResumeSectionModel
    {
        public ResumeSectionModel()
        {            
        }

        public int Id { get; set; }

        public int WebPageId { get; set; }

        public string Description { get; set; }
        public string ApplicationUserName { get; set; }
    }

    public class ResumeSectionReadOnlyModel : BaseReadOnlyModel
    {
        public string ApplicationUserName { get; set; }

        public ResumeSectionReadOnlyModel() {  }

        public ResumeSectionReadOnlyModel(Section section, IWebPageService webPageService, IHttpContextAccessor httpContextAccessor)
            : base(section, webPageService, httpContextAccessor) { }
    }
}