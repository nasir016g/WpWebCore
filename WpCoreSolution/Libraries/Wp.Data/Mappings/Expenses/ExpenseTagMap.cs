using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wp.Core.Domain.Expenses;

namespace Wp.Data.Mappings.Expenses
{
    public class ExpenseTagMap : EntityBaseConfiguration<ExpenseTag>
    {
        public override void Configure(EntityTypeBuilder<ExpenseTag> builder)
        {
            base.Configure(builder);
            builder.ToTable(nameof(ExpenseTag));
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).HasMaxLength(400).IsRequired();

        }
    }
}
