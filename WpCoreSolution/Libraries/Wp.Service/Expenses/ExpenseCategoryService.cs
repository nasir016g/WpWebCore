
using Wp.Core.Domain.Expenses;
using System.Linq;
using Wp.Data;
using Wp.Core;

namespace Wp.Services.Expenses
{
    public class ExpenseCategoryService : EntityService<ExpenseCategory>, IExpenseCategoryService
    {
        private readonly IUnitOfWork unitOfWork;
        private IBaseRepository<ExpenseCategory> expenseCategoryRepo;

        public ExpenseCategoryService(IUnitOfWork unitOfWork, 
            IBaseRepository<ExpenseCategory> expenseCategoryRepo) : base(unitOfWork, expenseCategoryRepo)
        {
            this.unitOfWork = unitOfWork;
            this.expenseCategoryRepo = expenseCategoryRepo;
        }

        public ExpenseCategory GetByName(string name)
        {
            return expenseCategoryRepo.Table.Where(x => x.Name.ToLower() == name.ToLower()).FirstOrDefault();
        }
    }
}
