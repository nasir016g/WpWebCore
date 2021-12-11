using System.IO;
using System.Threading.Tasks;
using Wp.Wh.Core.Domain;

namespace Wp.Wh.Services.ExportImport
{
    public partial interface IExportManager
    {
        Task<string> ExportResumeToXml(Resume entity);
    }
}