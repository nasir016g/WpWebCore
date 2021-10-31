using Nsr.Common.Core;
using Wp.Wh.Core.Domain;

namespace Wp.Wh.Services
{
    public interface IResumeService : IEntityService<Resume>
    {
        Resume GetByUserName(string name);
        Resume GetDetails(int id);
    }
}