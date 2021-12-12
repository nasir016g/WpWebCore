using System;
using System.Threading.Tasks;


namespace Nsr.Localization.Web.Data
{
    public interface ILocalizationUnitOfWork : IDisposable
    {
        int Complete();
        Task<int> CompleteAsync();
    }
    public class LocalizationUnitOfWork : ILocalizationUnitOfWork
    {
        private readonly LocalizationDbContext _context;

        public LocalizationUnitOfWork(LocalizationDbContext context)
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
