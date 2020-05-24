using System;
using System.Collections.Generic;
using System.Text;

namespace Wp.Core.Domain.Expenses
{
    public class ExpenseTag : Entity
    {
        private ICollection<ExpenseExpenseTagMapping> _expenseExpenseTagMappings;
        public string Name { get; set; }

        public virtual ICollection<ExpenseExpenseTagMapping> ExpenseExpenseTagMappings
        {
            get => _expenseExpenseTagMappings ?? (_expenseExpenseTagMappings = new List<ExpenseExpenseTagMapping>());
            protected set => _expenseExpenseTagMappings = value;
        }
    }
}
