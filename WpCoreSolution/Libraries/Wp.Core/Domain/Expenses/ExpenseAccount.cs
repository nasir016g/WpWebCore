using System.Collections.Generic;

namespace Wp.Core.Domain.Expenses
{
    public class ExpenseAccount : EntityAuditable
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Account { get; set; }

        private ICollection<Expense> _expenses;
        public virtual ICollection<Expense> Expenses
        {
            get { return _expenses ?? (_expenses = new List<Expense>()); }
            set { _expenses = value; }
        }
    }
}
