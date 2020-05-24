using System.Collections.Generic;

namespace Wp.Core.Domain.Expenses
{
    public class ExpenseAccount : EntityAuditable
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Account { get; set; }

        private ICollection<Expense> _expense;
        public ICollection<Expense> Expenses
        {
            get { return _expense ?? (_expense = new List<Expense>()); }
            set { _expense = value; }
        }
    }
}
