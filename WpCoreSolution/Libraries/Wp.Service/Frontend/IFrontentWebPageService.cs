using System.Collections.Generic;
using Wp.Core;
using Wp.Core.Domain.Frontend;
using Wp.Core.Domain.WebPages;

namespace Wp.Services.Frontend
{
    public interface IFrontendEntityService<T, SourceT> 
    where T : FrontendEntity
    where SourceT: Entity
    {
        T GetById(int id);
        void Insert(T entity);
        void Delete(T entity);
        IList<T> GetAll();
        void Update(T entity);

        void Insert(SourceT source);
        void Update(SourceT source);
        void Delete(SourceT source);
    }
    public interface IFrontentWebPageService : IFrontendEntityService<FrontendWebPage, WebPage>
    {


    }
}
