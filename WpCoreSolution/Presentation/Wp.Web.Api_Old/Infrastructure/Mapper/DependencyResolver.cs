using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using Wp.Core.Domain.WebPages;
using Wp.Services.WebPages;
using Wp.Web.Api.Models;

namespace Wp.Web.Api.Infrastructure.Mapper
{
    //public class DependencyResolver : IValueResolver<WebPage, WebPageModel, IEnumerable<WebPageModel.WebPageRoleModel>>
    //{
    //    private readonly IWebPageService _service;

    //    public DependencyResolver(IWebPageService service)
    //    {
    //        _service = service;
    //    }

    //    public IEnumerable<WebPageModel.WebPageRoleModel> Resolve(WebPage source, WebPageModel destination, IEnumerable<WebPageModel.WebPageRoleModel> destMember, ResolutionContext context)
    //    {
    //        var roles = _service.GetRolesByPageId(source.Id).ToList();
    //        var roleModelList = new List<WebPageModel.WebPageRoleModel>();
    //        roles.ForEach(x =>
    //        {
    //            roleModelList.Add(new WebPageModel.WebPageRoleModel { Id = x.Id, Name = x.Name  });
    //        });
    //        return roleModelList;
    //    }
    //}
}
