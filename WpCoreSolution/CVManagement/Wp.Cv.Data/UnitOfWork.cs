using System.Threading.Tasks;
using Wp.Common;


namespace Wp.Cv.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly WpCvDbContext _context;

        public UnitOfWork(WpCvDbContext context)
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
