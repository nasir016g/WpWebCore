using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wp.Core;
using Wp.Core.Domain.Localization;
using Wp.Common;

namespace Wp.Cv.Core.Domain
{
    public class SkillItem : EntityAuditable, ILocalizedEntity
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public string LevelDescription { get; set; }

        public int SkillId { get; set; }
        public virtual Skill Skill { get; set; }

    }
}
