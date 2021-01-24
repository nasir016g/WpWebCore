using System.Threading.Tasks;
using Wp.Common;


namespace Wp.Resumes.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly WpResumeDbContext _context;

        public UnitOfWork(WpResumeDbContext context)
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
