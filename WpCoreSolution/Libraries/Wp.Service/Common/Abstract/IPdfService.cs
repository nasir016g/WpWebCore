using System.IO;
using Wp.Core.Domain.Career;

namespace Wp.Services.Common
{
    public interface IPdfService
    {
        void PrintResume(Stream stream, Resume r);
    }
}