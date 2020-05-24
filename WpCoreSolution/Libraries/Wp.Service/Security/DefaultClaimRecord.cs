using System;
using System.Collections.Generic;
using System.Text;

namespace Wp.Service.Security
{
    public class DefaultClaimRecord
    {
        public DefaultClaimRecord()
        {
            this.ClaimRecords = new List<ClaimRecord>();
        }

        public string RoleName { get; set; }
        public IEnumerable<ClaimRecord> ClaimRecords { get; set; }
    }
}
