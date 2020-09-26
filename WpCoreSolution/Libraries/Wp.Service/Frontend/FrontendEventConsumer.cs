using System;
using System.Collections.Generic;
using System.Text;
using Wp.Core.Domain.WebPages;
using Wp.Core.Events;
using Wp.Services.Events;

namespace Wp.Services.Frontend
{
    public class FrontendEventConsumer :
        IConsumer<EntityInsertedEvent<WebPage>>,
        IConsumer<EntityUpdatedEvent<WebPage>>,
        IConsumer<EntityDeletedEvent<WebPage>>
    {
        private readonly IFrontentWebPageService _frontendWebPageService;

        public FrontendEventConsumer(IFrontentWebPageService frontendWebPageService)
        {
            _frontendWebPageService = frontendWebPageService;
        }
        public void HandleEvent(EntityInsertedEvent<WebPage> eventMessage)
        {
            _frontendWebPageService.Insert(eventMessage.Entity);
        }
        public void HandleEvent(EntityUpdatedEvent<WebPage> eventMessage)
        {
            _frontendWebPageService.Update(eventMessage.Entity);
        }       

        public void HandleEvent(EntityDeletedEvent<WebPage> eventMessage)
        {
            _frontendWebPageService.Delete(eventMessage.Entity);
        }
    }
}
