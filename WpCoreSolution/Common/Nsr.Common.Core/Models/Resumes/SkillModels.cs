using Nsr.Common.Core.Localization.Models;
using System.Collections.Generic;

namespace Nsr.Common.Core.Models
{
    #region group

    public class SkillModel : ILocalizedModel<SkillLocalizedModel>
    {
        public SkillModel()
        {
            Locales = new List<SkillLocalizedModel>();
        }
        public int Id { get; set; }
        public int ResumeId { get; set; }
        public string Name { get; set; }
        public int DisplayOrder { get; set; }
        public IList<SkillLocalizedModel> Locales { get; set; }
    }

    public class SkillLocalizedModel : ILocalizedModelLocal
    {
        public int LanguageId { get; set; }
        public string Name { get; set; }
    }

    #endregion

    #region items

    public class SkillItemModel : ILocalizedModel<SkillItemLocalizedModel>
    {
        public SkillItemModel()
        {
            Locales = new List<SkillItemLocalizedModel>();
        }
        public int Id { get; set; }
        public int SkillId { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public string LevelDescription { get; set; }
        public IList<SkillItemLocalizedModel> Locales { get; set; }
    }

    public class SkillItemLocalizedModel : ILocalizedModelLocal
    {
        public int LanguageId { get; set; }
        public string Name { get; set; }
        public string LevelDescription { get; set; }
    }

    #endregion
}