using Microsoft.EntityFrameworkCore;
using System.Linq;
using Wp.Core.Domain.WebPages;
using Wp.Core.Interfaces.Repositories;

namespace Wp.Data.Repositories
{
    //public class WebPageRepository : BaseRepository<WebPage>, IWebPageRepository
    //{
    //    public WebPageRepository(WpContext context) : base(context)
    //    {

    //    }

    //    public override WebPage GetById(int id)
    //    {
    //        var query = Context.WebPages
    //            //.Include(x => x.Roles)
    //            .Where(x => x.Id == id).FirstOrDefault();

    //        return query;
    //    }



    //    public WpContext ApplicationContext
    //    {
    //        get { return Context as WpContext; }
    //    }
    //}
}