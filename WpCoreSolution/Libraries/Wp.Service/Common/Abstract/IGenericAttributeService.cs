using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wp.Core;
using Wp.Core.Domain.Common;

namespace Wp.Services.Common
{
    public partial interface IGenericAttributeService
    {
        
        void DeleteAttribute(GenericAttribute attribute);
      
        void DeleteAttributes(IList<GenericAttribute> attributes);

        GenericAttribute GetAttributeById(int attributeId);
        
        void InsertAttribute(GenericAttribute attribute);

      
        void UpdateAttribute(GenericAttribute attribute);

      
        IList<GenericAttribute> GetAttributesForEntity(int entityId, string keyGroup);

       
        void SaveAttribute<TPropType>(Entity entity, string key, TPropType value);
    }
}
