using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wp.Core.Domain.Localization;

namespace Wp.Core.Domain.Career
{
    public class EducationItem : EntityAuditable, ILocalizedEntity
    {       
        public string Name { get; set; }
        public string Place { get; set; }
        public string Period { get; set; }
        public string Description { get; set; }

        public int EducationId { get; set; }
        public virtual Education Education { get; set; }
    }
}
