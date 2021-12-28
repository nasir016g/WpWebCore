namespace Nsr.ActivityLogs.Web.Data
{ 

    public interface IActivityLogUnitOfWork : IDisposable
    {
        int Complete();
        Task<int> CompleteAsync();
    }
    public class ActivityLogUnitOfWork : IActivityLogUnitOfWork
    {
        private readonly ActivityLogDbContext _context;

        public ActivityLogUnitOfWork(ActivityLogDbContext context)
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
