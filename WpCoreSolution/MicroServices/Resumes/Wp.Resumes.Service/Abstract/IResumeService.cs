using Wp.Common;
using Wp.Resumes.Core.Domain;

namespace Wp.Resumes.Services
{
    public interface IResumeService : IEntityService<Resume>
    {
        Resume GetByUserName(string name);
    }
}