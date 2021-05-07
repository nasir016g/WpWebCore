using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wp.Web.Mvc.Profile.Models;

namespace Wp.Web.Mvc.Profile.RestClients
{
    public interface ISkillWebApi
    {
        [Get("/skill/GetByResumeId/{resumeId}")]
        Task<List<SkillAdminModel>> GetSkills([AliasAs("resumeId")] int resumeId);

        [Get("/skill/{id}")]
        Task<SkillAdminModel> GetSkillById([AliasAs("id")] int id);

        [Put("/skill/{id}")]
        Task Update(int id, SkillAdminModel model);

        [Delete("/skill/{id}")]
        Task Delete([AliasAs("id")] int id);

        [Get("/skill/items/{skillId}")]
        Task<List<SkillItemAdminModel>> GetItems(int skillId);

        [Get("/skill/item/{id}")]
        Task<SkillItemAdminModel> GetItem(int id);

        [Put("/skill/item/{id}")]
        Task UpdateItem(int id, SkillItemAdminModel model);

        [Delete("/skill/item/{id}")]
        Task DeleteItem([AliasAs("id")] int id);
    }
}
