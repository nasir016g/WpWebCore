using Nsr.Common.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nsr.RestClient.Models.ActivityLogs
{
    public class ActivityLogType : Entity
    {
        /// <summary>
        /// Gets or sets the system keyword
        /// </summary>
        public string SystemKeyword { get; set; }

        /// <summary>
        /// Gets or sets the display name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the activity log type is enabled
        /// </summary>
        public bool Enabled { get; set; }
    }
}
