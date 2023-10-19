using System.IO;
using System.Threading.Tasks;
using Nsr.Work.Web.Domain;

namespace Nsr.Work.Web.Services.ExportImport
{
    public partial interface IExportManager
    {
        string ExportResumeToXml(Resume entity);
    }
}