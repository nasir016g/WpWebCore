using System.Threading.Tasks;
using Wp.Common;


namespace Wp.Localization.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly WpLocalizationDbContext _context;

        public UnitOfWork(WpLocalizationDbContext context)
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
