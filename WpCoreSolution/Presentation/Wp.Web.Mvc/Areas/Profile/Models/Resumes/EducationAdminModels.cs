﻿using Nsr.Common.Core.Localization.Models;
using System.Collections.Generic;

namespace Wp.Web.Mvc.Profile.Models
{
    #region group
    public class EducationAdminModel : ILocalizedModel<EducationLocalizedModel>
    {
        public EducationAdminModel()
        {
            Locales = new List<EducationLocalizedModel>();
        }
        public int Id { get; set; }
        public int ResumeId { get; set; }
        public string Name { get; set; }
        public IList<EducationLocalizedModel> Locales { get; set; }
    }

    public class EducationLocalizedModel : ILocalizedModelLocal
    {
        public int LanguageId { get; set; }
        public string Name { get; set; }
    }

    #endregion

    #region items    

    public class EducationItemAdminModel : ILocalizedModel<EducationItemLocalizedModel>
    {
        public EducationItemAdminModel()
        {
            Locales = new List<EducationItemLocalizedModel>();
        }
        public int Id { get; set; }
        public int EducationId { get; set; }
        public string Name { get; set; }
        public string Place { get; set; }
        public string Period { get; set; }
        public string Description { get; set; }

        public int ResumeId { get; set; }
        public IList<EducationItemLocalizedModel> Locales { get; set; }
    }

    public class EducationItemLocalizedModel : ILocalizedModelLocal
    {
        public int LanguageId { get; set; }
        public string Name { get; set; }
        public string Place { get; set; }
        public string Period { get; set; }
        public string Description { get; set; }
    }

    #endregion
}