using System;
using System.Threading.Tasks;


namespace Nsr.Localizations.Data
{
    public interface INsrLocalizationUnitOfWork : IDisposable
    {
        int Complete();
        Task<int> CompleteAsync();
    }
    public class NsrLocalizationUnitOfWork : INsrLocalizationUnitOfWork
    {
        private readonly NsrLocalizationDbContext _context;

        public NsrLocalizationUnitOfWork(NsrLocalizationDbContext context)
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
