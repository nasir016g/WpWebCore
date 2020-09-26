using System;
using System.Collections.Generic;
using System.Text;

namespace Wp.Core.Events
{
    public class EntityInsertedEvent<T> where T : Entity
    {
        public EntityInsertedEvent(T entity)
        {
            Entity = entity;
        }

        public T Entity { get; }
    }
}
