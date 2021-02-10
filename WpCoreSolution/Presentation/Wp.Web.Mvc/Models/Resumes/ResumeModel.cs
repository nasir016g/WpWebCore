using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wp.Web.Framework;

namespace Wp.Web.Mvc.Models.Resumes
{
    public class ResumeModel
    {
        public ResumeModel()
        {
            Educations = new List<EducationModel>();
            Skills = new List<SkillModel>();
            Experiences = new List<ExperienceModel>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string Town { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }         
        public string Email { get; set; }                 
        public string Website { get; set; }
        public string LinkedIn { get; set; }

        [WpResourceDisplayName("Common.DateOfBirth")]
        public string DateOfBirth { get; set; }

        [WpResourceDisplayName("Resume.Fields.Summary")]
        public string Summary { get; set; }

         [WpResourceDisplayName("Resume.Fields.Educations")]
        public IList<EducationModel> Educations { get; set; }

        [WpResourceDisplayName("Resume.Fields.Skills")]
        public IList<SkillModel> Skills { get; set; }

         [WpResourceDisplayName("Resume.Fields.Experiences")]
        public IList<ExperienceModel> Experiences { get; set; }
    }


}