using System.IO;
using Wp.Resumes.Core.Domain;

namespace Wp.Resumes.Services.ExportImport
{
    public partial interface IExportManager
    {
        string ExportResumeToXml(Resume entity);
        void ExportResumeToWord(Stream stream, Resume c);
    }
}