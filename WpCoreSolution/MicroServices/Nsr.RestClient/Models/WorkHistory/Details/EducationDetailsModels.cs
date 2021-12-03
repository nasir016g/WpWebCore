using Nsr.Common.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nsr.RestClient.Models.WorkHistories
{
    public class EducationDetailsModels : IEntity, ILocalizedEntity
    {
        public EducationDetailsModels()
        {
            EducationItems = new List<EducationDetialItemModel>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public IList<EducationDetialItemModel> EducationItems { get; set; }
    }

    public class EducationDetialItemModel : IEntity, ILocalizedEntity
    {
        public EducationDetialItemModel()
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