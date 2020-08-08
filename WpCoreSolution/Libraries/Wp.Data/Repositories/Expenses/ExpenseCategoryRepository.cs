using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wp.Core.Domain.Expenses;
using Wp.Core.Interfaces.Repositories;

namespace Wp.Data.Repositories
{
    public class ExpenseCategoryRepository : BaseRepository<ExpenseCategory>, IExpenseCategoryRepository
    {
        public ExpenseCategoryRepository(WpDbContext context) : base(context)
        {
        }

        public override ExpenseCategory GetById(int id)
        {
            var query = Context.Set<ExpenseCategory>()
                    .Include(x => x.Expenses);
            return query.FirstOrDefault(x => x.Id == id);
        }

        public override IQueryable<ExpenseCategory> Table
        {
            get
            {
                var query = Context.Set<ExpenseCategory>()
                    .Include(x => x.Expenses);
                return query;
            }
        }

        public WpDbContext ApplicationContext
        {
            get { return Context as WpDbContext; }
        }
    }


}
