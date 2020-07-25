using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wp.Web.Api.Models.Admin
{
    public class TenantModel
    {
        public int Id { get; set; }
        public Guid TenantId { get; set; }
        public string TenantName { get; set; }
        public string ConnectionString { get; set; }
    }
}
