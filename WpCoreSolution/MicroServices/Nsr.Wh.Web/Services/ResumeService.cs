using System.Linq;
using Nsr.Common.Core;
using Nsr.Wh.Web.Domain;
using Microsoft.EntityFrameworkCore;

namespace Nsr.Wh.Web.Services
{
    public class ResumeService : EntityService<Resume>, IResumeService
    {
        private IBaseRepository<Resume> _resumeRepo;

        public ResumeService(IUnitOfWork unitOfWork, IBaseRepository<Resume> resumeRepo) : base(unitOfWork, resumeRepo)
        {
            this._resumeRepo = resumeRepo;
        }

       
        public Resume GetByUserName(string userName)
        {
            return _resumeRepo.Table.Where(x => x.ApplicationUserName == userName).FirstOrDefault();
        }

        /// <summary>
        /// Includes Eductions, Experiences and Skills
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Resume GetDetails(int id)
        {
            return _resumeRepo.Table.Where(x => x.Id == id)
                .Include(x => x.Experiences).ThenInclude(e => e.Projects)
                .Include(x => x.Educations).ThenInclude(e => e.EducationItems)
                .Include(x => x.Skills).ThenInclude(s => s.SkillItems)
                .FirstOrDefault();
        }
    }
}
