using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Wp.Services.ExportImport
{
    public interface IImportExcelService
    {
        void ImportExpensesFromXlsx(Stream stream);
    }
}
