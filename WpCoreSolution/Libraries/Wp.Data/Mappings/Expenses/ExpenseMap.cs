using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wp.Core.Domain.Expenses;

namespace Wp.Data.Mappings.Expenses
{
    public class ExpenseMap : EntityBaseConfiguration<Expense>
    {
        public override void Configure(EntityTypeBuilder<Expense> builder)
        {
            base.Configure(builder);
            builder.ToTable("Expense");
            builder.Property(e => e.Name).IsRequired().HasMaxLength(200);
            builder.Property(e => e.Description);
            builder.HasOne(e => e.ExpenseCategory)
                .WithMany(ec => ec.Expenses)
                .HasForeignKey(e => e.ExpenseCategoryId);

            builder.HasOne(e => e.ExpenseAccount)
               .WithMany(ea => ea.Expenses)
               .HasForeignKey(e => e.ExpenseAccountId);
        }       
    }
}
