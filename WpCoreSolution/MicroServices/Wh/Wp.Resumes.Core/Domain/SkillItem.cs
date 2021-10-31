using Nsr.Common.Core;

namespace Wp.Wh.Core.Domain
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
