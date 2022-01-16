using Microsoft.EntityFrameworkCore;
using Nsr.ActivityLogs.Web.Data;
using Nsr.ActivityLogs.Web.Data.Repositories;
using Nsr.ActivityLogs.Web.Service.Abstract;
using Nsr.Common.Core;
using Nsr.RestClient.Models.ActivityLogs;

namespace Nsr.ActivityLogs.Web.Service
{
    public class ActivityLogService : EntityService<ActivityLog>, IActivityLogService
    {
        private readonly IActivityLogBaseRepository<ActivityLogType> _activityLogTypeRepository;
        private readonly IActivityLogBaseRepository<ActivityLogItem> _activityLogItemRepository;

        public ActivityLogService(IActivityLogUnitOfWork unitOfWork, IActivityLogBaseRepository<ActivityLog> repository, IActivityLogBaseRepository<ActivityLogType> activityLogTypeRepository, IActivityLogBaseRepository<ActivityLogItem> activityLogItemRepository) : base(unitOfWork, repository)
        {
            _activityLogTypeRepository = activityLogTypeRepository;
            _activityLogItemRepository = activityLogItemRepository;
        }

        public override ActivityLog GetById(int id)
        {
           return _repository.Table.Include(x => x.ActivityLogItems).FirstOrDefault(x => x.Id == id);            
        }

        public ActivityLog InsertActivity(string systemKeyword, string entityType, int entityId)
        {
           
            //try to get activity log type by passed system keyword
            var activityLogType = _activityLogTypeRepository.Table.FirstOrDefault(type => type.SystemKeyword.Equals(systemKeyword));
            if (!activityLogType?.Enabled ?? true)
                return null;

            //insert log item
            var logItem = new ActivityLog
            {
                ActivityLogTypeId = activityLogType.Id,
                EntityId = entityId,
                EntityName = entityType,
                Comment = activityLogType.Name,
                CreatedOnUtc = DateTime.UtcNow               
            };
            Insert(logItem);

            return logItem;
        }

        public void InsertActivity(List<AuditEntry> auditEntries)
        {
            foreach (var auditEntry in auditEntries)
            {
                string systemKeyword = null;
                switch (auditEntry.AuditType)
                {
                    case AuditType.None:
                        break;
                    case AuditType.Create:
                        systemKeyword = $"AddNew{auditEntry.EntityType}";
                        break;
                    case AuditType.Update:
                        systemKeyword = $"Edit{auditEntry.EntityType}";
                        break;
                    case AuditType.Delete:
                        systemKeyword = $"Delete{auditEntry.EntityType}";
                        break;
                    default:
                        break;
                }

                var activityLogType = _activityLogTypeRepository.Table.FirstOrDefault(type => type.SystemKeyword.Equals(systemKeyword));
                if (!activityLogType?.Enabled ?? true)
                    return;

                //insert log item
                var logItem = new ActivityLog
                {
                    ActivityLogTypeId = activityLogType.Id,
                    EntityId = auditEntry.EntityId,
                    EntityName = auditEntry.EntityType,
                    Comment = activityLogType.Name,
                    CreatedOnUtc = DateTime.UtcNow
                };
               

                List<ActivityLogItem> items = new();
                foreach (var p in auditEntry.Properties)
                {
                    var item = new ActivityLogItem
                    {
                        EntityId = auditEntry.EntityId,
                        EntityKey = auditEntry.EntityType,
                        ActivityLog = logItem,
                        NewValue = p.NewValue.ToString(),
                        OldValue = p.OldValue.ToString()
                    };
                    items.Add(item);
                }
                logItem.ActivityLogItems = items;
                Insert(logItem);
                //_activityLogItemRepository.AddRange(items);
                _unitOfWork.Complete();

            }
           

        }   

    }
}
