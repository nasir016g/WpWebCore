using System;
using System.Collections.Generic;
using System.Text;
using Wp.Core;
using Wp.Core.Domain.Expenses;
using System.Linq;

namespace Wp.Services.Expenses
{
    public class ExpenseTagService : EntityService<ExpenseTag>, IExpenseTagService
    {
        private readonly IBaseRepository<ExpenseTag> _ExpenseTagRepository;
        private readonly IBaseRepository<ExpenseExpenseTagMapping> _expenseExpenseTagRepository;
        private readonly IExpenseService _expenseService;

        public ExpenseTagService(IUnitOfWork unitOfWork, IBaseRepository<ExpenseTag> repository, IBaseRepository<ExpenseExpenseTagMapping> _expenseExpenseTagRepository, IExpenseService expenseService) : base(unitOfWork, repository)
        {
            _ExpenseTagRepository = repository;
            this._expenseExpenseTagRepository = _expenseExpenseTagRepository;
            _expenseService = expenseService;
        }

        public IList<ExpenseTag> GetAllExpenseTagsByExpenseId(int expenseId)
        {
            var query = from et in _ExpenseTagRepository.Table
                        join eet in _expenseExpenseTagRepository.Table on et.Id equals eet.ExpenseTagId
                        where eet.ExpenseId == expenseId
                        orderby et.Id
                        select et;

            var expenseTags = query.ToList();
            return expenseTags;
        }

        

        public ExpenseTag GetExpenseTagByName(string name)
        {
            return _ExpenseTagRepository.Table.FirstOrDefault(x => x.Name == name);
        }

        public void UpdateExpenseTags(Expense expense, string[] expenseTags)
        {
            if (expense == null)
                throw new ArgumentNullException(nameof(expense));

            var existingTags = GetAllExpenseTagsByExpenseId(expense.Id);
            var expenseTagsToRemove = new List<ExpenseTag>();
            foreach (var existingTag in existingTags)
            {
                var found = false;
                foreach (var newTag in expenseTags)
                {
                    if (!existingTag.Name.Equals(newTag, StringComparison.InvariantCultureIgnoreCase))
                        continue;

                    found = true;
                    break;
                }

                if (!found)
                    expenseTagsToRemove.Add(existingTag);
            }

            // remove tags from expense 
            foreach (var expenseTag in expenseTagsToRemove)
            {
                expense.ExpenseExpenseTagMappings.Remove(expense.ExpenseExpenseTagMappings.FirstOrDefault(x => x.ExpenseTagId == expenseTag.Id));
                _expenseService.Update(expense);
            }

            foreach (var tagName in expenseTags)
            {
                ExpenseTag tag;
                var tag2 = GetExpenseTagByName(tagName);
                if (tag2 == null)
                {
                    tag = new ExpenseTag
                    {
                        Name = tagName
                    };
                    Insert(tag);
                }
                else
                {
                    tag = tag2;
                }

                if(!_expenseService.ExpenseTagExists(expense, tag.Id))
                {
                    expense.ExpenseExpenseTagMappings.Add(new ExpenseExpenseTagMapping { ExpenseTag = tag });
                    _expenseService.Update(expense);
                }
            }
        }       
    }
}
