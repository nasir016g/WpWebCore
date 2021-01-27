using System;
using System.Threading.Tasks;


namespace Nsr.Common.Data
{
    public interface ICommonUnitOfWork : IDisposable
    {
        int Complete();
        Task<int> CompleteAsync();
    }
    public class CommonUnitOfWork : ICommonUnitOfWork
    {
        private readonly WpCommonDbContext _context;

        public CommonUnitOfWork(WpCommonDbContext context)
        {
            _context = context;
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }        

        public void Dispose()
        {
           // _context.Dispose();
        }
    }
}
