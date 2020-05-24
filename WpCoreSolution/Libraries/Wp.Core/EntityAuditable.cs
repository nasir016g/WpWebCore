using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wp.Core
{
    public abstract class EntityAuditable : Entity, IEntityAuditable
    {
        public DateTime CreatedOn {get;set;}
        public DateTime UpdatedOn { get; set; }       
    }
}
