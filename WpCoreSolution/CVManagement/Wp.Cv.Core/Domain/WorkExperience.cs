﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wp.Core;
using Wp.Core.Domain.Localization;
using Wp.Common;


namespace Wp.Cv.Core.Domain
{
    public class Experience : EntityAuditable, ILocalizedEntity
    {
        public string Name { get; set; }
        public string Period { get; set; }
        public string Function { get; set; }
        public string City { get; set; }
        public string Tasks { get; set; }
        public int DisplayOrder { get; set; }

        public int ResumeId { get; set; }
        public virtual Resume Resume { get; set; }

        private ICollection<Project> _projects;
        public virtual ICollection<Project> Projects 
        {
            get {return _projects ?? (_projects = new List<Project>()); }
            set { _projects = value; }
        }
    }
}
