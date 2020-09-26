using System;
using System.Collections.Generic;
using System.Text;
using Wp.Core;
using Wp.Core.Events;

namespace Wp.Services.Events
{
    public static class EventPublisherExtensions
    {
        public static void EntityInserted<T>(this IEventPublisher eventPublisher, T entity) where T : Entity
        {
            eventPublisher.Publish(new EntityInsertedEvent<T>(entity));
        }

        public static void EntityUpdated<T>(this IEventPublisher eventPublisher, T entity) where T : Entity
        {
            eventPublisher.Publish(new EntityUpdatedEvent<T>(entity));
        }

        public static void EntityDeleted<T>(this IEventPublisher eventPublisher, T entity) where T : Entity
        {
            eventPublisher.Publish(new EntityDeletedEvent<T>(entity));
        }
    }
}
