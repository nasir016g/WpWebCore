using System;
using System.Threading.Tasks;

namespace Nsr.Common.Core
{
    public interface IUnitOfWork : IDisposable
    {
        // ISettingRepository Settings { get; }
        int Complete();
        Task<int> CompleteAsync();
    }
}
