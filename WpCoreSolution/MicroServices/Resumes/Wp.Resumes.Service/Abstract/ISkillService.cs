using System.Collections.Generic;
using Wp.Common;
using Wp.Resumes.Core.Domain;

namespace Wp.Resumes.Services
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