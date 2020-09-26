using Wp.Core.Domain.Frontend;
using Wp.Core.Domain.WebPages;
using Wp.Data;
using Wp.Data.Repositories.Frontend;
using Wp.Services.Frontend;

namespace Wp.Services.Presentation
{
    public class FrontendWebPageService : FrontendEntityService<FrontendWebPage, WebPage>, IFrontentWebPageService
    {
        public FrontendWebPageService(FrontendDbContext frontendDbContext, IFrontendBaseRepository<FrontendWebPage> repository) : base(frontendDbContext, repository)
        {
        }

        public override void Delete(WebPage source)
        {
            var entity = GetById(source.Id);
            if(entity != null)
                Delete(entity);
        }

        public override void Insert(WebPage source)
        {
            var entity = GetById(source.Id);
            if (entity == null)
            {
                entity = new FrontendWebPage
                {
                    Id = source.Id,
                    VirtualPath = source.VirtualPath
                };
                Insert(entity);
            }
        }

        public override void Update(WebPage source)
        {
            var entity = GetById(source.Id);
            if (entity != null)
            {
                entity.Id = source.Id;
                entity.VirtualPath = source.VirtualPath;
                Update(entity);
            }
        }
    }
}
