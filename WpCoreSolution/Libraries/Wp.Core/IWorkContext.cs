using Wp.Core.Domain.Common;

namespace Wp.Core
{
    public interface IWorkContext
    {
        WorkContextModel Current { get; }
    }
}
