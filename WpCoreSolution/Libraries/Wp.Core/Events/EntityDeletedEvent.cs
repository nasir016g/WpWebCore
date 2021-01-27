using System;
using System.Collections.Generic;
using System.Text;
using Nsr.Common.Core;

namespace Wp.Core.Events
{
    public class EntityDeletedEvent<T> where T : Entity
    {
        public EntityDeletedEvent(T entity)
        {
            Entity = entity;
        }

        public T Entity { get;}
    }
}
