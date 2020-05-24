using System.Collections.Generic;
using Wp.Core.Domain.Localization;

namespace Wp.Core.Domain.Common
{
    public partial class CustomAttribute : EntityAuditable, ILocalizedEntity
    {
        private ICollection<CustomAttributeValue> _customAttributeValues;

        public string Name { get; set; }       
        public bool IsRequired { get; set; }

        public int AttributeControlTypeId { get; set; }       
        public int DisplayOrder { get; set; }

        public AttributeControlType AttributeControlType
        {
            get
            {
                return (AttributeControlType)this.AttributeControlTypeId;
            }
            set
            {
                this.AttributeControlTypeId = (int)value;
            }
        }
       
        public virtual ICollection<CustomAttributeValue> CustomAttributeValues
        {
            get { return _customAttributeValues ?? (_customAttributeValues = new List<CustomAttributeValue>()); }
            protected set { _customAttributeValues = value; }
        }
    }
}
