using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wp.Core.Domain.Localization
{
    public class LocalizedProperty : Entity
    {
        public int EntityId { get; set; }        
        public string LocaleKeyGroup { get; set; }
        public string LocaleKey { get; set; }
        public string LocaleValue { get; set; }

        public int LanguageId { get; set; }
        public virtual Language Language { get; set; }
    }
}
