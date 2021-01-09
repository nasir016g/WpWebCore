using System;

namespace Wp.Common
{
    public abstract class EntityAuditable : Entity, IEntityAuditable
    {
        public DateTime CreatedOn {get;set;}
        public DateTime UpdatedOn { get; set; }       
    }
}
