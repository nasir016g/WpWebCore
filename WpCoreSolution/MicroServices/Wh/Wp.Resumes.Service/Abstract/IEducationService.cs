using System.Collections.Generic;
using Nsr.Common.Core;
using Wp.Wh.Core.Domain;

namespace Wp.Wh.Services
{
    public interface IEducationService : IEntityService<Education>
    {
        IList<Education> GetAll(int ResumeId);

        IList<EducationItem> GetEducationItemsByEducationId(int educationId);
        EducationItem GetEducationItemById(int id);
        void InsertEducationItem(EducationItem t);
        void UpdateEducationItem(EducationItem t);
        void DeleteEducationItem(EducationItem t);
    }
}