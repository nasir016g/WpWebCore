using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wp.Core.Security;

namespace Wp.Data.Mappings.Security
{
    public class ApplicationUserMap 
    {
        public ApplicationUserMap(EntityTypeBuilder<ApplicationUser> entityBuilder)
        {
            entityBuilder.HasOne(x => x.Resume);
             
        }
    }
}
