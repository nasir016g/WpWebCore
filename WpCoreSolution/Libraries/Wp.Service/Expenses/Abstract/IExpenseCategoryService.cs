using Wp.Core;
using Wp.Core.Domain.Expenses;

namespace Wp.Services.Expenses
{
    public interface IExpenseCategoryService : IEntityService<ExpenseCategory>
    {
        ExpenseCategory GetByName(string name);
    }
}