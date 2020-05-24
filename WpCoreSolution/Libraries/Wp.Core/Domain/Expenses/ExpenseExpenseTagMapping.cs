using System;
using System.Collections.Generic;
using System.Text;

namespace Wp.Core.Domain.Expenses
{
   public class ExpenseExpenseTagMapping : Entity
    {
        public int ExpenseId { get; set; }
        public int ExpenseTagId { get; set; }
        public virtual Expense Expense { get; set; }
        public virtual ExpenseTag ExpenseTag { get; set; }
    }
}
