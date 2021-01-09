using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wp.Resume.ManagementApi.Models;

namespace Wp.Resume.ManagementApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ResumeController : Controller
    {
        [HttpGet]
        public IEnumerable<ResumeModel> Index()
        {
            List<ResumeModel> list = new List<ResumeModel>();
            for (int i = 0; i < 5; i++)
            {
                list.Add(new ResumeModel() { Name = $"Resume {i}" });
            }
            return list;
        }
    }
}
