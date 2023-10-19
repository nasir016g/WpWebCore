using System.IO;
using Nsr.Work.Web.Domain;

namespace Nsr.Work.Web.Services
{
    public interface IPdfService
    {
        void PrintResume(Stream stream, Resume r);
    }
}