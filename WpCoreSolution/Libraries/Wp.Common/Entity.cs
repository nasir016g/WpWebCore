using System;

namespace Wp.Common
{
    public abstract class Entity : EntityBase, IEntity
    {
        
        public virtual int Id { get; set; }        
       
    }
}
