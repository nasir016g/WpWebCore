using System;
using System.Threading.Tasks;
using Wp.Common;


namespace Wp.Localization.Data
{
    public interface ILocalizationUnitOfWork : IDisposable
    {
        int Complete();
        Task<int> CompleteAsync();
    }
    public class LocalizationUnitOfWork : ILocalizationUnitOfWork
    {
        private readonly WpLocalizationDbContext _context;

        public LocalizationUnitOfWork(WpLocalizationDbContext context)
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
