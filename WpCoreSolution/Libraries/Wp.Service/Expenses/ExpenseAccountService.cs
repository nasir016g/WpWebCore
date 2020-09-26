
using Wp.Core.Domain.Expenses;
using System.Linq;
using Wp.Data;
using Wp.Core;
using Wp.Services.Events;

namespace Wp.Services.Expenses
{
    public class ExpenseAccountService : EntityService<ExpenseAccount>, IExpenseAccountService
    {
        private readonly IUnitOfWork unitOfWork;
        private IBaseRepository<ExpenseAccount> expenseAccountRepo;

        public ExpenseAccountService(IUnitOfWork unitOfWork,
            IBaseRepository<ExpenseAccount> expenseAccountRepo, IEventPublisher eventPublisher)
            : base(unitOfWork, expenseAccountRepo, eventPublisher)
        {
            this.unitOfWork = unitOfWork;
            this.expenseAccountRepo = expenseAccountRepo;
        }

        public ExpenseAccount GetByAccount(string account)
        {
            return expenseAccountRepo.Table.Where(x => x.Account == account).FirstOrDefault();
        }

        public ExpenseAccount GetByName(string name)
        {
            return expenseAccountRepo.Table.Where(x => x.Name.ToLower() == name.ToLower()).FirstOrDefault();
        }


    }
}
