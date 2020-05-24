using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Wp.Core.Domain.Expenses;

namespace Wp.Data.Mappings.Expenses
{
    public class ExpenseExpenseTagMap : EntityBaseConfiguration<ExpenseExpenseTagMapping>
    {
        public override void Configure(EntityTypeBuilder<ExpenseExpenseTagMapping> builder)
        {
            base.Configure(builder);
            builder.ToTable("Expense_ExpenseTag_Mapping");
            builder.HasKey(x => new { x.ExpenseId, x.ExpenseTagId });

            builder.Property(x => x.ExpenseId).HasColumnName("Expense_Id");
            builder.Property(x => x.ExpenseTagId).HasColumnName("ExpenseTag_Id");

            builder.HasOne(x => x.Expense)
                .WithMany(expense => expense.ExpenseExpenseTagMappings)
                .HasForeignKey(x => x.ExpenseId)
                .IsRequired();

            builder.HasOne(x => x.ExpenseTag)
                .WithMany(expenseTag => expenseTag.ExpenseExpenseTagMappings)
                .HasForeignKey(x => x.ExpenseTagId)
                .IsRequired();

            builder.Ignore(x => x.Id);
        }
    }
}
