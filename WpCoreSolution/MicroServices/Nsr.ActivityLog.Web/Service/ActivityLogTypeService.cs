using Nsr.ActivityLogs.Web.Data;
using Nsr.ActivityLogs.Web.Data.Repositories;
using Nsr.ActivityLogs.Web.Service.Abstract;
using Nsr.RestClient.Models.ActivityLogs;

namespace Nsr.ActivityLogs.Web.Service
{
  
    public class ActivityLogTypeService : EntityService<ActivityLogType>, IActivityLogTypeService
    {
        public ActivityLogTypeService(IActivityLogUnitOfWork unitOfWork, IActivityLogBaseRepository<ActivityLogType> repository) : base(unitOfWork, repository)
        {
        }
    }
}
