using Wp.Core;
using Wp.Core.Domain.Sections;

namespace Wp.Services.Sections
{
    public interface ISectionService : IEntityService<Section>
    {
       // Section GetById(int id);

        /// <summary>
        /// Installed sections
        /// </summary>
        /// <returns>list of sections</returns>
        string[] GetAvailableSections();

    }
}