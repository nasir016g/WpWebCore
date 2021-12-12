using System.IO;
using Nsr.Wh.Web.Domain;

namespace Nsr.Wh.Web.Services
{
    public interface IPdfService
    {
        void PrintResume(Stream stream, Resume r);
    }
}