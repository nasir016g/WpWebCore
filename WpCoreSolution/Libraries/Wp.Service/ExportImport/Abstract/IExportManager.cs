using System.IO;
using Wp.Core.Domain.Career;

namespace Wp.Services.ExportImport
{
    public partial interface IExportManager
    {
        string ExportResumeToXml(Resume entity);
        void ExportResumeToWord(Stream stream, Resume c);
    }
}