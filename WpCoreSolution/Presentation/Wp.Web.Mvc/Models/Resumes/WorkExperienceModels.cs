using Nsr.Common.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wp.Web.Framework;

namespace Wp.Web.Mvc.Models.Resumes
{
    public class ExperienceModel : IEntity, ILocalizedEntity
    {
        public ExperienceModel()
        {
            Projects = new List<ProjectModel>();
        }

        public int Id { get; set; }

        [WpResourceDisplayName("Resume.Fields.Experiences.Name")]
        public string Name { get; set; }

        [WpResourceDisplayName("Resume.Fields.Experiences.Period")]
        public string Period { get; set; }

        [WpResourceDisplayName("Resume.Fields.Experiences.Function")]
        public string Function { get; set; }

        [WpResourceDisplayName("Resume.Fields.Experiences.City")]
        public string City { get; set; }

        [WpResourceDisplayName("Resume.Fields.Experiences.Tasks")]
        public string Tasks { get; set; }

        public int DisplayOrder { get; set; }

        public IList<ProjectModel> Projects { get; set; }
    }

    public class ProjectModel : IEntity, ILocalizedEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Technology { get; set; }
    }
}