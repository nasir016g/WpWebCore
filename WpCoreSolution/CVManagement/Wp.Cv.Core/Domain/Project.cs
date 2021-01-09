using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wp.Common;
using Wp.Core;
using Wp.Core.Domain.Localization;

namespace Wp.Cv.Core.Domain
{
    public class Project : EntityAuditable, ILocalizedEntity
    {            
        public string Name { get; set; }
        public string Description { get; set; }
        public string Technology { get; set; }

        public int ExperienceId { get; set; }
        public virtual Experience Experience { get; set; }
    }
}
