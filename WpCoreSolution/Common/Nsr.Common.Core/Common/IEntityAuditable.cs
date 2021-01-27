using System;

namespace Nsr.Common.Core
{
    public interface IEntityAuditable
    {
        DateTime CreatedOn { get; set; }
        DateTime UpdatedOn { get; set; }
    }
}
