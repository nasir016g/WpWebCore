using Wp.Wh.Core.Domain;

namespace Wp.Wh.Services.ExportImport
{
    public interface IImportManager
    {
        Resume ImportWorkFromXml(string content, string applicationUserName);
    }
}