using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Wp.Services.Expenses;
using Wp.Services.ExportImport;
using Wp.Services.Models;
using Wp.Web.Api.Extensions.Mapper;
using Wp.Web.Api.Models;
using Wp.Web.Api.Models.Admin;

namespace Wp.Web.Api.Areas.Admin.Controllers
{
    [Route("api/admin/[controller]")]
    //[ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseService _expenseService;
        private readonly IExpenseAccountService _expenseAccountService;
        private readonly IExpenseCategoryService _expenseCategoryService;
        private readonly IExpenseTagService _expenseTagService;
        private readonly IImportManager _importManager;

        #region ctor
        public ExpenseController(IExpenseService expenseService, IExpenseAccountService expenseAccountService, IExpenseCategoryService expenseCategoryService, IExpenseTagService expenseTagService, IImportManager importManager)
        {
            _expenseService = expenseService;
            _expenseAccountService = expenseAccountService;
            _expenseCategoryService = expenseCategoryService;
            _expenseTagService = expenseTagService;
            _importManager = importManager;
        }
        #endregion

        #region utilities
        private ExpenseModel PrepareModel(ExpenseModel model)
        {
            model.ExpenseTags = string.Join(", ", _expenseTagService.GetAllExpenseTagsByExpenseId(model.Id).Select(x => x.Name));
            return model;
        }
        #endregion
        // GET: api/Expense
        [HttpGet]
        public IActionResult Get()
        {
            var entities = _expenseService.GetAll();
            var models = entities.ToModels();
            return Ok(models);
        }

        [HttpPost("search")]
        public IActionResult Search([FromBody]ExpenseSearchModel searchModel)
        {
            var pagedList = _expenseService.GetAll(searchModel);
            var models = pagedList.ToModels();
            foreach (var m in models)
            {
                PrepareModel(m);
            }

            var searchResultModel = new SearchResultModel<ExpenseModel>()
            {
                Data = models,
                TotalRecords = pagedList.TotalRecords
            };

            return Ok(searchResultModel);
        }        

        [HttpPost("searchTotals")]
        public IActionResult SearchTotals([FromBody]ExpenseSearchModel searchModel)
        {
            var rez = _expenseService.GetSearchTotals(searchModel);

            return Ok(rez);
        }

        // GET: api/Expense/5
        [HttpGet("{id}", Name = "GetExpense")]
        public IActionResult Get(int id)
        {
            var entity = _expenseService.GetById(id);
            var model = entity.ToModel();
            PrepareModel(model);

            return Ok(model);
        }

        // POST: api/Expense  
        [HttpPost]
        public IActionResult Post([FromBody] ExpenseModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var entity = model.ToEntity();
            entity.Id = 0;
            _expenseService.Insert(entity);
            return NoContent();
        }

        // PUT: api/Expense/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] ExpenseModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var entity = _expenseService.GetById(id);
            entity = model.ToEntity(entity);

            var expenseAccount = _expenseAccountService.GetByName(model.ExpenseAccount.Name);
            entity.ExpenseAccount = expenseAccount;
            entity.ExpenseAccountId = expenseAccount.Id;

            var expenseCategory = _expenseCategoryService.GetByName(model.ExpenseCategory.Name);
            entity.ExpenseCategory = expenseCategory;
            entity.ExpenseCategoryId = expenseCategory.Id;

            _expenseService.Update(entity);
            _expenseTagService.UpdateExpenseTags(entity, model.ExpenseTags.ParseExpenseTags());
            return NoContent();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var entity = _expenseService.GetById(id);
            _expenseService.Delete(entity);
        }

        [HttpPost("{importexcelfile}", Name = "ImportFromXlsx")]
        public virtual IActionResult ImportFromXlsx(IFormFile importexcelfile)
        {

            try
            {
                if (importexcelfile != null && importexcelfile.Length > 0)
                {
                    _importManager.ImportExpensesFromXlsx(importexcelfile.OpenReadStream());
                }
                else
                {
                    return BadRequest("No file.");
                }

                return Ok();
            }
            catch (Exception exc)
            {
                return BadRequest(exc);
            }
        }
    }
}
