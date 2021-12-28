namespace Nsr.RestClient.Models.ActivityLogs
{
    public class AuditEntry
    {
        public string UserId { get; set; }
        public string EntityType { get; set; }
        public int EntityId { get; set; }
        //public Dictionary<string, object> OldValues { get; } = new Dictionary<string, object>();
        //public Dictionary<string, object> NewValues { get; } = new Dictionary<string, object>();
        public List<AuditEntryProperty> Properties { get; set; } = new List<AuditEntryProperty>();
        public AuditType AuditType { get; set; }
        public List<string> ChangedColumns { get; } = new List<string>();
        
    }

    public class AuditEntryProperty
    {
        public string Name { get; set; }
        public object OldValue { get; set; }    
        public object NewValue { get; set; }

    }
}
