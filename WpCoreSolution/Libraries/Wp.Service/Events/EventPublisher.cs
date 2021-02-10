using Nsr.Common.Core.Localization;
using System;
using System.Linq;
namespace Wp.Services.Events
{
    public partial class EventPublisher : IEventPublisher
    {
        public void Publish<TEvent>(TEvent @event)
        {
          var consumers = ServiceLocator.ResolveAll<IConsumer<TEvent>>().ToList();

            foreach (var consumer in consumers)
            {
                try
                {
                    consumer.HandleEvent(@event);
                }
                catch (Exception )
                {
                    throw;
                    ////log error, we put in to nested try-catch to prevent possible cyclic (if some error occurs)
                    //try
                    //{
                    //   // EngineContext.Current.Resolve<ILogger>()?.Error(exception.Message, exception);
                    //}
                    //catch { }
                }
            }
        }
    }
}
