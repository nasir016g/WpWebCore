using System;
using System.Collections.Generic;
using System.Text;

namespace Wp.Core.Events
{
    public class EntityUpdatedEvent<T> where T : Entity
    {
        public EntityUpdatedEvent(T entity)
        {
            Entity = entity;
        }

        public T Entity { get; }
    }
}
