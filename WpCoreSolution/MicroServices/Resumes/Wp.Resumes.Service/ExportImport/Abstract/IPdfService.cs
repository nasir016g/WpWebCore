using System.IO;
using Wp.Resumes.Core.Domain;

namespace Wp.Resumes.Services
{
    public interface IPdfService
    {
        void PrintResume(Stream stream, Resume r);
    }
}