using Nsr.Work.Web.Domain;

namespace Nsr.Work.Web.Services.ExportImport
{
    public interface IImportManager
    {
        Resume ImportWorkFromXml(string content, string applicationUserName);
    }
}