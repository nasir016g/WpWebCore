using Wp.Core;
using Wp.Core.Domain.Career;

namespace Wp.Services.Career
{
    public interface IResumeService : IEntityService<Resume>
    {
        Resume GetByUserName(string name);
    }
}