using Wp.Core.Domain.Localization;

namespace Wp.Core.Domain.Common
{
    public partial class CustomAttributeValue : EntityAuditable, ILocalizedEntity
    {       
        public int CustomAttributeId { get; set; }        
        public string Name { get; set; }        
        public bool IsPreSelected { get; set; }       
        public int DisplayOrder { get; set; }       
        public virtual CustomAttribute CustomAttribute { get; set; }
    }
}