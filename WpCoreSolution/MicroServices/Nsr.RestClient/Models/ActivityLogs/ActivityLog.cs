using Nsr.Common.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nsr.RestClient.Models.ActivityLogs
{
    public class ActivityLog : Entity
    {
        public int ActivityLogTypeId { get; set; }
        public int? EntityId { get; set; }
        public string EntityName { get; set; }        
        public string Comment { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public virtual ActivityLogType ActivityLogType { get; set; }
        private ICollection<ActivityLogItem> _activityLogItems;
        public ICollection<ActivityLogItem> ActivityLogItems
        {
            get { return _activityLogItems ??= new List<ActivityLogItem>(); }
            set { _activityLogItems = value; }
        }


        
    }
}
