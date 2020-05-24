using System;

namespace Wp.Core
{
    public abstract class Entity : EntityBase, IEntity
    {
        
        public virtual int Id { get; set; }        
       
    }
}
