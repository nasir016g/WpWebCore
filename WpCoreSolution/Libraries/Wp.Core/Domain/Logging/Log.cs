using System;

namespace Wp.Core.Domain.Logging
{
    /// <summary>
    /// Represents a log record
    /// </summary>
    public partial class Log : EntityAuditable
    {
        /// <summary>
        /// Gets or sets the log level 
        /// </summary>
        public string Level { get; set; }

        /// <summary>
        /// Gets or sets the short message
        /// </summary>
        public string ShortMessage { get; set; }

        /// <summary>
        /// Gets or sets the full exception
        /// </summary>
        public string FullMessage { get; set; }

        /// <summary>
        /// Gets or sets the IP address
        /// </summary>
        public string IpAddress { get; set; }        

        /// <summary>
        /// Gets or sets the page URL
        /// </summary>
        public string PageUrl { get; set; }

        /// <summary>
        /// Gets or sets the referrer URL
        /// </summary>
        public string ReferrerUrl { get; set; } 

        public LogLevel LogLevel { get; set; }
             
    }
}
