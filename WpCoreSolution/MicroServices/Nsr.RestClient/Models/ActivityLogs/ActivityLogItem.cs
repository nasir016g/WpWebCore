using Microsoft.AspNetCore.Html;
using Nsr.Common.Core;
using System.ComponentModel.DataAnnotations.Schema;

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
       
        [NotMapped]
        public string Diff { get; set; }
        public virtual ActivityLog ActivityLog { get; set; }

    }
}
