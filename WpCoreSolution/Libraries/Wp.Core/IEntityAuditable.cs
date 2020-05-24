using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wp.Core
{
    public interface IEntityAuditable
    {
        DateTime CreatedOn { get; set; }
        DateTime UpdatedOn { get; set; }
    }
}
