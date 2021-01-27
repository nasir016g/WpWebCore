using System.Collections.Generic;
using Nsr.Common.Core;
using Wp.Core;
using Wp.Core.Domain.Common;

namespace Wp.Services.Common
{
    public partial interface ICustomAttributeService : 
        IEntityService<CustomAttribute>
    {
      
      
        IList<CustomAttributeValue> GetCustomAttributeValues(int customAttributeId);

        CustomAttributeValue GetCustomAttributeValueById(int customAttributeValueId);

        void InsertCustomAttributeValue(CustomAttributeValue customAttributeValue);

      
        void UpdateCustomAttributeValue(CustomAttributeValue customAttributeValue);
    }
}