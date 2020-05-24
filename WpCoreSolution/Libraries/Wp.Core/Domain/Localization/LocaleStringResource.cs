using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wp.Core.Domain.Localization
{
    public class LocaleStringResource: Entity
    {
        public string ResourceName { get; set; }       
        public string ResourceValue { get; set; }
        public int LanguageId { get; set; }        
        public virtual Language Language { get; set; }
    }
}
