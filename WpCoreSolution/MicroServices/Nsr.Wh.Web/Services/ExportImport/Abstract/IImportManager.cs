using Nsr.Wh.Web.Domain;

namespace Nsr.Wh.Web.Services.ExportImport
{
    public interface IImportManager
    {
        Resume ImportWorkFromXml(string content, string applicationUserName);
    }
}