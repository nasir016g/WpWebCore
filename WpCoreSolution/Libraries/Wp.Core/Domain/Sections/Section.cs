using Wp.Core.Domain.Localization;
using Wp.Core.Domain.WebPages;

namespace Wp.Core.Domain.Sections
{
    //abstract
    public abstract class Section : EntityAuditable, ILocalizedEntity
    {
        public string Description { get; set; }

        public int WebPageId { get; set; }
        public virtual WebPage WebPage { get; set; }
    }   
}

   
