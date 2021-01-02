using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel;


namespace Wp.Web.Framework.Models.Admin
{
    public class WebsiteSettingsModel
    {
        public string WebsiteName { get; set; }
        public string FooterText { get; set; }
        //public string Theme { get; set; }

        //email
        public string SmtpServer { get; set; }
        public string SmtpUser { get; set; }
        public string SmtpPassword { get; set; }
        public string SmtpDomain { get; set; }
        public string MailSenderAddress { get; set; }

        //seo
        [DisplayName("Meta title")]
        public string Title { get; set; }

        [DisplayName("Meta keywords")]
        public string MetaKeywords { get; set; }

        [DisplayName("Meta description")]
        public string MetaDescription { get; set; }

        public IEnumerable<SelectListItem> AvailableThemes { get; set; }
        public string SelectedTheme { get; set; }
    }
}