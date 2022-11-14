﻿using System.IO;
using System.Threading.Tasks;
using Nsr.Wh.Web.Domain;

namespace Nsr.Wh.Web.Services.ExportImport
{
    public partial interface IExportManager
    {
        string ExportResumeToXml(Resume entity);
    }
}