using Nsr.Common.Core;
using Nsr.RestClient.Models.ActivityLogs;

namespace Nsr.ActivityLogs.Web.Service.Abstract
{
    public interface IActivityLogService : IEntityService<ActivityLog>
    {
        ActivityLog InsertActivity(string systemKeyword, string entityType, int entityId);
        void InsertActivity(List<AuditEntry> auditEntries);
    }
}
