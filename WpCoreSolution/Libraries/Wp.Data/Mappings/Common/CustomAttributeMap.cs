using Wp.Core.Domain.Common;

namespace Wp.Data.Mappings.Common
{
    public partial class CustomAttributeMap : EntityBaseConfiguration<CustomAttribute>
    {
        public CustomAttributeMap()
        {
            this.ToTable("CustomAttribute");
            this.HasKey(aa => aa.Id);
            this.Property(aa => aa.Name).IsRequired().HasMaxLength(400);

            this.Ignore(aa => aa.AttributeControlType);
        }
    }
}
