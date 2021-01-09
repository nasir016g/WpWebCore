using System;

namespace Wp.Common
{
    public interface IEntityAuditable
    {
        DateTime CreatedOn { get; set; }
        DateTime UpdatedOn { get; set; }
    }
}
