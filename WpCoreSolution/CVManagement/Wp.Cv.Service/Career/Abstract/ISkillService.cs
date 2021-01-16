using System.Collections.Generic;
using Wp.Common;
using Wp.Cv.Core.Domain;

namespace Wp.Services.Career
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