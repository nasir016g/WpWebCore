using System.IO;
using Wp.Wh.Core.Domain;

namespace Wp.Wh.Services.ExportImport
{
    public partial interface IExportManager
    {
        string ExportResumeToXml(Resume entity);
    }
}