using System.Collections.Generic;
using Nsr.Common.Core;
using Nsr.Work.Web.Domain;

namespace Nsr.Work.Web.Services
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