using System.Collections.Generic;

namespace Wp.Core.Domain.Expenses
{
    public class ExpenseCategory : EntityAuditable
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }

        private ICollection<Expense> _expenses;
        public virtual ICollection<Expense> Expenses
        {
            get { return _expenses ?? (_expenses = new List<Expense>()); }
            set { _expenses = value; }
        }
    }
}
