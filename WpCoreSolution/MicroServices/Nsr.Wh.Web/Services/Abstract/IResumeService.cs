using Nsr.Common.Core;
using Nsr.Wh.Web.Domain;

namespace Nsr.Wh.Web.Services
{
    public interface IResumeService : IEntityService<Resume>
    {
        Resume GetByUserName(string name);
        Resume GetDetails(int id);
    }
}