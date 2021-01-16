﻿using System.Collections.Generic;
using Wp.Common;

namespace Wp.Cv.Core.Domain
{
    public class Education : EntityAuditable, ILocalizedEntity
    {            
        public string Name { get; set; }

        public int ResumeId { get; set; }
        public virtual Resume Resume { get; set; }

        private ICollection<EducationItem> _educationItems;
        public virtual ICollection<EducationItem> EducationItems
        {
            get { return _educationItems ?? (_educationItems = new List<EducationItem>()); }
            set { _educationItems = value; }
        }
    }
}
