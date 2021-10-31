using System.Collections.Generic;
using Nsr.Common.Core;
using Wp.Wh.Core.Domain;

namespace Wp.Wh.Services
{
    public interface ISkillService : IEntityService<Skill>
    {
        IList<Skill> GetAll(int ResumeId);

        IList<SkillItem> GetSkillItemsBySkillId(int skillId);
        SkillItem GetSkillItemById(int id);
        void InsertSkillItem(SkillItem t);
        void UpdateSkillItem(SkillItem t);
        void DeleteSkillItem(SkillItem t);
    }
}