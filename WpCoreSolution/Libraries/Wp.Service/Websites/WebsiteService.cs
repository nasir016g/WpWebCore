using Nsr.Common.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wp.Core.Domain.Websites;
using Wp.Services.Events;

namespace Wp.Services.Websites
{
    public class WebsiteService : EntityService<Website>, IWebsiteService
    {
        public WebsiteService(IUnitOfWork unitOfWork, IBaseRepository<Website> repository, IEventPublisher eventPublisher) : base(unitOfWork, repository, eventPublisher)
        {
        }
    }
}
