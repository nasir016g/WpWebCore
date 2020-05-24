using System;
using System.Collections.Generic;
using System.Text;
using Wp.Core.Domain.Expenses;

namespace Wp.Core.Interfaces.Repositories
{
    public interface IExpenseRepository : IBaseRepository<Expense>
    {
    }
}
