using Nsr.Common.Core;

namespace Nsr.Work.Web.Domain
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
