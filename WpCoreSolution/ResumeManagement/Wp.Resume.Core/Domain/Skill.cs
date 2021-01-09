using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wp.Core.Domain.Localization;

namespace Wp.Core.Domain.Career
{
    public class Skill : EntityAuditable, ILocalizedEntity
    {        
        public string Name { get; set; }
        public int DisplayOrder { get; set; }

        public int ResumeId { get; set; }
        public virtual Resume Resume { get; set; }

        private ICollection<SkillItem> _skillItems;
        public virtual ICollection<SkillItem> SkillItems
        {
            get { return _skillItems ?? (_skillItems = new List<SkillItem>()); }
            set { _skillItems = value; }
        }
    }
}
