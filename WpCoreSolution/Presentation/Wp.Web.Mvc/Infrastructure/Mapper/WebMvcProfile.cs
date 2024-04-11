using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wp.Core.Domain.Sections;
using Wp.Web.Mvc.Models.Sections;

namespace Wp.Web.Mvc.Infrastructure.Mapper
{
    public class WebMvcProfile : AutoMapper.Profile
    {
        public WebMvcProfile()
        {
            CreateMap<ContactFormSection, ContactFormSectionModel>()
             .ReverseMap();

            CreateMap<HtmlContentSection, HtmlContentSectionModel>()
              .ReverseMap();

            CreateMap<WorkHistorySection, WorkHistorySectionModels>()
                .ReverseMap();
        }
    }
}
