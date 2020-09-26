using System;
using System.Collections.Generic;
using System.Text;

namespace Wp.Services.Events
{
    public partial interface IEventPublisher
    {
        void Publish<TEvent>(TEvent @event);
    }
}
