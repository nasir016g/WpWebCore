using System.Collections.Generic;
using System.Linq;
using Wp.Core;
using Wp.Core.Domain.Career;
using Wp.Data;
using Wp.Services.Events;

namespace Wp.Services.Career
{  
    public class ResumeService : EntityService<Resume>, IResumeService
    {
        private IBaseRepository<Resume> _resumeRepo;

        public ResumeService(IUnitOfWork unitOfWork, IBaseRepository<Resume> resumeRepo, IEventPublisher eventPublisher) : base(unitOfWork, resumeRepo, eventPublisher)
        {
            this._resumeRepo = resumeRepo;
        }

       
        public Resume GetByUserName(string userName)
        {
            return _resumeRepo.Table.Where(x => x.ApplicationUserName == userName).FirstOrDefault();
        }
    }
}
