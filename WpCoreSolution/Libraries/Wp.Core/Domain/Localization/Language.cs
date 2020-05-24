using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wp.Core.Domain.Localization
{
    public class Language : EntityAuditable
    {        
        public string Name { get; set; }       
        public string LanguageCulture { get; set; }        
        public string UniqueSeoCode { get; set; }       
        public string FlagImageFileName { get; set; }
        public bool Rtl { get; set; }
        public bool Published { get; set; }
        public int DisplayOrder { get; set; }

        private ICollection<LocaleStringResource> _localeStringResources;
        public virtual ICollection<LocaleStringResource> LocaleStringResources
        {
            get { return _localeStringResources ?? (_localeStringResources = new List<LocaleStringResource>()); }
            protected set { _localeStringResources = value; }
        }
    }
}
