using Wp.Common;
using Wp.Cv.Core.Domain;

namespace Wp.Services.Career
{
    public interface IResumeService : IEntityService<Resume>
    {
        Resume GetByUserName(string name);
    }
}