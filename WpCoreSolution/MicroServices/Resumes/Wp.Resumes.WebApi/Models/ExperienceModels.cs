﻿using System.Collections.Generic;
using Wp.Localization.Core.Models;

namespace Wp.Resumes.WebApi.Models
{
    #region group

    public class ExperienceModel : ILocalizedModel<ExperienceLocalizedModel>
    {
        public ExperienceModel()
        {
            Locales = new List<ExperienceLocalizedModel>();
        }
        public int Id { get; set; }
        public int ResumeId { get; set; }
        public string Name { get; set; }
        public string Period { get; set; }
        public string Function { get; set; }
        public string City { get; set; }
        public string Tasks { get; set; }
        public int DisplayOrder { get; set; }
        public IList<ExperienceLocalizedModel> Locales { get; set; }
    }

    public class ExperienceLocalizedModel : ILocalizedModelLocal
    {
        public int LanguageId { get; set; }
        public string Name { get; set; }
        public string Period { get; set; }
        public string Function { get; set; }
        public string Tasks { get; set; }
    }


    #endregion 

    #region projects

    public class ProjectModel : ILocalizedModel<ProjectLocalizedModel>
    {
        public ProjectModel()
        {
            Locales = new List<ProjectLocalizedModel>();
        }
        public int Id { get; set; }
        public int ExperienceId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Technology { get; set; }
        public IList<ProjectLocalizedModel> Locales { get; set; }
    }

    public class ProjectLocalizedModel : ILocalizedModelLocal
    {
        public int LanguageId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Technology { get; set; }
    }

    #endregion
}