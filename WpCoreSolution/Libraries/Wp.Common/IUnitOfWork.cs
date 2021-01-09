using System;
using System.Threading.Tasks;

namespace Wp.Common
{
    public interface IUnitOfWork : IDisposable
    {
        // ISettingRepository Settings { get; }
        int Complete();
        Task<int> CompleteAsync();
    }
}
