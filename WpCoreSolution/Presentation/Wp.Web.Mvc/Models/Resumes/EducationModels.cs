﻿using Nsr.Common.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wp.Web.Mvc.Models.Resumes
{
    public class EducationModel : IEntity, ILocalizedEntity
    {
        public EducationModel()
        {
            EducationItems = new List<EducationItemModel>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public IList<EducationItemModel> EducationItems { get; set; }
    }

    public class EducationItemModel : IEntity, ILocalizedEntity
    {
        public EducationItemModel()
        {
            Descriptions = new List<string>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Place { get; set; }
        public string Period { get; set; }
        public string Description { get; set; } // entity
        public List<string> Descriptions { get; set; } // view model // Refactor later to use one of them
    }
}