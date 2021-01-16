﻿using Wp.Common;

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
