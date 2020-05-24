using Wp.Core;
using Wp.Core.Domain.Expenses;

namespace Wp.Services.Expenses
{
    public interface IExpenseAccountService : IEntityService<ExpenseAccount>
    {
        ExpenseAccount GetByName(string name);
        ExpenseAccount GetByAccount(string account);
    }
}