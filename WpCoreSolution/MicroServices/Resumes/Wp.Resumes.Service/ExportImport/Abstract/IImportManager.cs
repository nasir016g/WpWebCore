using Wp.Resumes.Core.Domain;

namespace Wp.Resumes.Services.ExportImport
{
    public interface IImportManager
    {
        Resume ImportWorkFromXml(string content, string applicationUserName);
    }
}