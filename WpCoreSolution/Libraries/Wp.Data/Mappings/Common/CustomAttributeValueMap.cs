using Wp.Core.Domain.Common;

namespace Wp.Data.Mappings.Common
{
    public partial class CustomAttributeValueMap : EntityBaseConfiguration<CustomAttributeValue>
    {
        public CustomAttributeValueMap()
        {
            this.ToTable("CustomAttributeValue");
            this.HasKey(aav => aav.Id);
            this.Property(aav => aav.Name).IsRequired().HasMaxLength(400);

            this.HasRequired(aav => aav.CustomAttribute)
                .WithMany(aa => aa.CustomAttributeValues)
                .HasForeignKey(aav => aav.CustomAttributeId);
        }
    }
}
