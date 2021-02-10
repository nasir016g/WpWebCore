using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wp.Web.Mvc.Models.Resumes
{
    public class EducationModel
    {
        public EducationModel()
        {
            Items = new List<EducationItemModel>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public IList<EducationItemModel> Items { get; set; }
    }

    public class EducationItemModel
    {
        public EducationItemModel()
        {
            Descriptions = new List<string>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Place { get; set; }
        public string Period { get; set; }
        public List<string> Descriptions { get; set; }
    }
}