using Nsr.Common.Core;
using Nsr.Work.Web.Domain;

namespace Nsr.Work.Web.Services
{
    public interface IResumeService : IEntityService<Resume>
    {
        Resume GetByUserName(string name);
        Resume GetDetails(int id);
    }
}