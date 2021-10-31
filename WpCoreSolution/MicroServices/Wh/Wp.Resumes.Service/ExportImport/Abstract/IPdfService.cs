using System.IO;
using Wp.Wh.Core.Domain;

namespace Wp.Wh.Services
{
    public interface IPdfService
    {
        void PrintResume(Stream stream, Resume r);
    }
}