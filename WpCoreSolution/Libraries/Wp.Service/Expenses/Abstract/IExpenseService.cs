using System;
using Wp.Core;
using Wp.Core.Domain.Expenses;
using Wp.Services.Models;

namespace Wp.Services.Expenses
{
    public interface IExpenseService : IEntityService<Expense>
    {
        IPagedList<Expense> GetAll(ExpenseSearchModel searchModel);
        ExpenseSearchTotalsModel GetSearchTotals(ExpenseSearchModel searchModel);
        Expense GetByDescription(string description, DateTime dateTime);
        bool ExpenseTagExists(Expense expense, int expenseTagId);
    }
}