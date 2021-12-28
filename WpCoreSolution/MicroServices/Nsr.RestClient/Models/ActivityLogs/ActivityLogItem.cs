using Nsr.Common.Core;

namespace Nsr.RestClient.Models.ActivityLogs
{
    public class ActivityLogItem : Entity
    {
        public int ActivityLogId { get; set; }
        public string EntityKey { get; set; }
        public int EntityId { get; set; }
        public int LanguageId { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }  
        public virtual ActivityLog ActivityLog { get; set; }

    }
}
