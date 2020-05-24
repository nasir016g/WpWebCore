using System.IO;
using Wp.Core.Domain.Career;
using Wp.Core.Security;

namespace Wp.Services.ExportImport
{
    public interface IImportManager
    {
        Resume ImportWorkFromXml(string content, ApplicationUser user);
        void ImportExpensesFromXlsx(Stream stream);
    }
}