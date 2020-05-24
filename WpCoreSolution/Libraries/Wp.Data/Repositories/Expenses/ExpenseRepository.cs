using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wp.Core.Domain.Expenses;
using Wp.Core.Interfaces.Repositories;

namespace Wp.Data.Repositories
{
    public class ExpenseRepository : BaseRepository<Expense>, IExpenseRepository
    {
        public ExpenseRepository(WpDbContext context) : base(context)
        {
        }

        public override Expense GetById(int id)
        {
            var query = Context.Set<Expense>()
                    .Include(x => x.ExpenseAccount)
                    .Include(x => x.ExpenseCategory)
                    .Include(x => x.ExpenseExpenseTagMappings);
            return query.FirstOrDefault(x => x.Id == id);
        }

        public override IQueryable<Expense> Table
        {
            get
            {
                var query = Context.Set<Expense>()
                    .Include(x => x.ExpenseAccount)
                    .Include(x => x.ExpenseCategory);
                return query;
            }
        }

        public WpDbContext ApplicationContext
        {
            get { return Context as WpDbContext; }
        }
    }


}
