using Nsr.Common.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wp.Core.Domain.Websites
{
    public class Website : EntityAuditable
    {
        public Website()
        {
            Theme = "Default";
        }

        public string WebsiteName { get; set; }
        public string FooterText { get; set; }
        public string Theme { get; set; }

        public string SmtpServer { get; set; }
        public string SmtpUser { get; set; }
        public string SmtpPassword { get; set; }
        public string SmtpDomain { get; set; }
        public string MailSenderAddress { get; set; }

        public string Title { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
    }
}
