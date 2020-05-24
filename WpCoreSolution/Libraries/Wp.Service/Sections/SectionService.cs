using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;
using System.Web;
using Wp.Core;
using Wp.Core.Domain.Sections;
using Wp.Data;
using Wp.Services;

namespace Wp.Services.Sections
{
  public class SectionService : EntityService<Section>, ISectionService
  {
      private readonly IBaseRepository<Section> _sectionRepo;
        private readonly IHostingEnvironment _hostingEnvironment;

        public SectionService(IUnitOfWork unitOfWork, IBaseRepository<Section> sectionRepository, IHostingEnvironment hostingEnvironment) : base(unitOfWork, sectionRepository)
    {
      this._sectionRepo = sectionRepository;
            _hostingEnvironment = hostingEnvironment;
    }

    //public Section GetById(int id)
    //{
    // return _sectionRepo.GetById(id);
    //}

    /// <summary>
    /// Installed sections
    /// </summary>
    /// <returns>list of sections</returns>
    public string[] GetAvailableSections()
    {
            //    //var path = _httpContext.HttpContext..Current.Server.MapPath("~/Views/Sections";
            //    string webRootPath = _hostingEnvironment.WebRootPath;
            //    string contentRootPath = _hostingEnvironment.ContentRootPath;
            //    var path = webRootPath + "\n" + contentRootPath;
            //    var sectionFolder = new DirectoryInfo(path);
            //var sections = sectionFolder.GetDirectories().Where(x => x.Name != "SectionBase" && !x.Name.StartsWith("_Old")).Select(x => x.Name).ToArray();

            string[] sections = new string[] { "ContactForm", "Html" };
            return sections;
    }    
  }  
}
