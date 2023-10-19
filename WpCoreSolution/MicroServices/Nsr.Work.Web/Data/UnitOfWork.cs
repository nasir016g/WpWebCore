using System.Threading.Tasks;
using Nsr.Common.Core;


namespace Nsr.Work.Web.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly WpWhDbContext _context;

        public UnitOfWork(WpWhDbContext context)
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
