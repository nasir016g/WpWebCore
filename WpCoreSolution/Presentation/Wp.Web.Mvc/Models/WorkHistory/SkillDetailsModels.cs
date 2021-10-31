using Nsr.Common.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wp.Web.Mvc.Models.WorkHistories
{
    public class SkillDetailsModels : IEntity, ILocalizedEntity
    {
        public SkillDetailsModels()
        {
            SkillItems = new List<SkillItemModel>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int DisplayOrder { get; set; }
        public IList<SkillItemModel> SkillItems { get; set; }
    }

    public class SkillItemModel : IEntity, ILocalizedEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }       
        public int Level { get; set; }
        public string LevelDescription { get; set; }
    }
}