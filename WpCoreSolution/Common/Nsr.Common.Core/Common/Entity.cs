using System;

namespace Nsr.Common.Core
{
    public abstract class Entity : EntityBase, IEntity
    {
        
        public virtual int Id { get; set; }        
       
    }
}
