using System.Collections.Generic;
using Wp.Core.Domain.Common;

namespace Wp.Services.Common
{
    public partial interface ICustomAttributeService
    {
        
        void DeleteCustomAttribute(CustomAttribute customAttribute);

       
        IList<CustomAttribute> GetAllCustomAttributes();
        
       

        CustomAttribute GetCustomAttributeById(int customttributeId);

        void InsertCustomAttribute(CustomAttribute customAttribute);

        void UpdateCustomAttribute(CustomAttribute customAttribute);

        
        void DeleteCustomAttributeValue(CustomAttributeValue customAttributeValue);

      
        IList<CustomAttributeValue> GetCustomAttributeValues(int customAttributeId);

        CustomAttributeValue GetCustomAttributeValueById(int customAttributeValueId);

        void InsertCustomAttributeValue(CustomAttributeValue customAttributeValue);

      
        void UpdateCustomAttributeValue(CustomAttributeValue customAttributeValue);
    }
}