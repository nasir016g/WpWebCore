
using Wp.Core;
using Wp.Core.Domain.Expenses;
using Wp.Core.Interfaces.Repositories;
using Wp.Data;
using System.Linq;
using System;
using System.Linq.Expressions;
using Wp.Services.Models;

namespace Wp.Services.Expenses
{
    public class ExpenseService : EntityService<Expense>, IExpenseService
    {
        private readonly IUnitOfWork unitOfWork;
        private IExpenseRepository _expenseRepo;
        private readonly IBaseRepository<ExpenseExpenseTagMapping> _expenseExpenseTagRepository;

        public ExpenseService(IUnitOfWork unitOfWork, 
            IExpenseRepository expenseRepo, 
            IBaseRepository<ExpenseExpenseTagMapping> expenseExpenseTagRepository): base(unitOfWork, expenseRepo)
        {
            this.unitOfWork = unitOfWork;
            _expenseRepo = expenseRepo;
            _expenseExpenseTagRepository = expenseExpenseTagRepository;
        }

        #region Utilities

        private IQueryable<Expense> SearchUseLinq(ExpenseSearchModel search)
        {
            search ??= new ExpenseSearchModel();
            var query = _expenseRepo.Table;

            if (!string.IsNullOrEmpty(search.Name))
            {
                query = query.Where(x => x.Name.ToLower().Contains(search.Name.ToLower()));
            }

            if (!string.IsNullOrEmpty(search.Description))
            {
                query = query.Where(x => x.Description.ToLower().Contains(search.Description.ToLower()));
            }
            if (search.DateFrom.HasValue)
            {
                DateTime localDate = search.DateFrom.Value.ToLocalTime();
                query = query.Where(x => x.Date.Date >= localDate.Date);
            }

            if (search.DateTo.HasValue)
            {
                DateTime localDate = search.DateTo.Value.ToLocalTime();
                query = query.Where(x => x.Date.Date <= localDate.Date);
            }

            //if(!string.IsNullOrEmpty(search.ExpenseTags))
            //{
            //  var ets = search.ExpenseTags.ParseExpenseTags();
            if (search.ExpenseTags != null && search.ExpenseTags.Count() > 0)
            {
                query = from e in query
                        join et in _expenseExpenseTagRepository.Table on e.Id equals et.ExpenseId
                        where search.ExpenseTags.Contains(et.ExpenseTag.Name)
                        select e;
            }

            if (search.ExpenseAccounts != null && search.ExpenseAccounts.Count() > 0)
            {
                query = from e in query
                        where search.ExpenseAccounts.Contains(e.ExpenseAccount.Name)
                        select e;
            }

            if (search.ExpenseCategories != null && search.ExpenseCategories.Count() > 0)
            {
                query = from e in query
                        where search.ExpenseCategories.Contains(e.ExpenseCategory.Name)
                        select e;
            }

            return query;
        }


        #endregion

        public bool ExpenseTagExists(Expense expense, int expenseTagId)
        {
            if (expense == null)
                throw new ArgumentNullException(nameof(expense));

          return  expense.ExpenseExpenseTagMappings.Any(x => x.ExpenseTagId == expenseTagId);
        }

        public IPagedList<Expense> GetAll(ExpenseSearchModel search = null)
        {
            search ??= new ExpenseSearchModel();
            IQueryable<Expense> query = SearchUseLinq(search);

            if (search?.SortField != null)
            {
                //// Sort based on descending or not
                //query = searchModel.SortDescending ? query.OrderByDescending(ExpressionHelper.GetSortProperty<Expense>(searchModel.SortField)) :
                //    query.OrderBy(ExpressionHelper.GetSortProperty<Expense>(searchModel.SortField));

                query = query.OrderBy(search.SortField, search.SortDescending);
            }

            return new PagedList<Expense>(query, search.PageIndex, search.PageSize);
        }

       
        public ExpenseSearchTotalsModel GetSearchTotals(ExpenseSearchModel search)
        {
            var expenseList = SearchUseLinq(search).ToList();
            ExpenseSearchTotalsModel model = new ExpenseSearchTotalsModel();
            model.TotalAmount = expenseList.Sum(x => x.Amount).ToString("0.00");
            model.SumNegative = expenseList.Where(x => x.Amount < 0).Sum(x => x.Amount).ToString("0.00");
            model.SumPositive = expenseList.Where(x => x.Amount > 0).Sum(x => x.Amount).ToString("0.00");

            return model;
        }

        public Expense GetByDescription(string description, DateTime dateTime)
        {
           return _expenseRepo.Table.SingleOrDefault(x => x.Description == description && x.Date.Date == dateTime.Date);
        }

        
    }
}
