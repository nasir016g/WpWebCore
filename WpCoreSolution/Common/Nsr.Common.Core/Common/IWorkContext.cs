

namespace Nsr.Common.Core
{
    public interface IWorkContext
    {
        WorkContextModel Current { get; set; }
        void ClearCurrentSession();

    }
}
