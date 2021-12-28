using Nsr.RestClient.Models.ActivityLogs;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nsr.RestClient.RestClients.ActivityLogs
{
    public interface IActivityLogWebApi
    {
        [Get("/api/ActivityLog")]
        Task<List<ActivityLog>> GetAll();

        [Get("/api/ActivityLog/{id}")]
        Task<ActivityLog> GetById([AliasAs("id")] int id);

        [Post("/api/ActivityLog/{systemKeyword}/{entityType}/{entityId}")]
        Task Insert(string systemKeyword, string entityType, int entityId);

        [Post("/api/ActivityLog/AuditEntries")]
        Task InsertAuditEntries([Body] List<AuditEntry> auditEntries);

        [Delete("/api/ActivityLog/{id}")]
        Task Delete([AliasAs("id")] int id);
    }
}
