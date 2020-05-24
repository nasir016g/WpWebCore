using System.Collections.Generic;
using Wp.Core.Domain.Sections;
using System.ComponentModel.DataAnnotations;
using Wp.Core.Domain.Seo;

namespace Wp.Core.Domain.WebPages
{
    public class WebPage : EntityAuditable, ISlugSupported
    {
        public WebPage()
        {
            Visible = true;
            AllowAnonymousAccess = true;
        }
        public int ParentId { get; set; }


        [Required]
        public string VirtualPath { get; set; }

        [Required]
        public string NavigationName { get; set; }
        public int DisplayOrder { get; set; }

        public bool Visible { get; set; }
        public bool AllowAnonymousAccess { get; set; }

        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public string MetaTitle { get; set; }

        private ICollection<Section> _sections;
        public virtual ICollection<Section> Sections
        {
            get { return _sections ?? (_sections = new List<Section>()); }
            set { _sections = value; }
        }

        private ICollection<WebPageRole> _roles;
        public virtual ICollection<WebPageRole> Roles
        {
            get { return _roles ?? (_roles = new List<WebPageRole>()); }
            set { _roles = value; }
        }

    }
}
