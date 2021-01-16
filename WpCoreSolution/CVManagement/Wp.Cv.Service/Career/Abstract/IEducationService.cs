using System.Collections.Generic;
using Wp.Common;
using Wp.Cv.Core.Domain;

namespace Wp.Cv.Services
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