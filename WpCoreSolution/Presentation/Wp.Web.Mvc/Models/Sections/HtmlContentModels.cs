using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nsr.Common.Core.Localization.Models;
using System.Collections.Generic;
using Wp.Core.Domain.Sections;
using Wp.Services.WebPages;
using Wp.Web.Framework.Localization;

namespace Wp.Web.Mvc.Models.Sections
{
    public class HtmlContentSectionModel : ILocalizedModel<HtmlContentLocalizedModel>
    {
        public HtmlContentSectionModel()
        {
            Locales = new List<HtmlContentLocalizedModel>();
        }

        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int WebPageId { get; set; }

        public string Description { get; set; }
        public string Html { get; set; }
        public IList<HtmlContentLocalizedModel> Locales { get; set; }
    }

    public class HtmlContentLocalizedModel : ILocalizedModelLocal
    {
        public int LanguageId { get; set; }

        //[AllowHtml]
        public string Html { get; set; }
    }

    public class HtmlContentSectionReadOnlyModel : BaseReadOnlyModel
    {
        public HtmlContentSectionReadOnlyModel()
        {
        }

        public HtmlContentSectionReadOnlyModel(Section section, IWebPageService webPageService, IHttpContextAccessor httpContextAccessor)
            : base(section, webPageService, httpContextAccessor) { }

       // [AllowHtml]
        public string Html { get; set; }
    }


}