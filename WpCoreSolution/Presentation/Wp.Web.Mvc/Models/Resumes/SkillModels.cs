using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wp.Web.Mvc.Models.Resumes
{
    public class SkillModel
    {
        public SkillModel()
        {
            Items = new List<SkillItemModel>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public IList<SkillItemModel> Items { get; set; }
    }

    public class SkillItemModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public string LevelDescription { get; set; }
    }
}