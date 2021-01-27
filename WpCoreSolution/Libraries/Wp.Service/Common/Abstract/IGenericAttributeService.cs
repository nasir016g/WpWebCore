using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nsr.Common.Core;
using Wp.Core;
using Wp.Core.Domain.Common;

namespace Wp.Services.Common
{
    public partial interface IGenericAttributeService : IEntityService<GenericAttribute>
    {
        
      
        void DeleteAttributes(IList<GenericAttribute> attributes);    

      

      
        IList<GenericAttribute> GetAttributesForEntity(int entityId, string keyGroup);

       
        void SaveAttribute<TPropType>(Entity entity, string key, TPropType value);
    }
}
