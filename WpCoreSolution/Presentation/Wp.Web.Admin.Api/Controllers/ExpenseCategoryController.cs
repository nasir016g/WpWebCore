using Microsoft.AspNetCore.Mvc;
using Wp.Services.Expenses;
using Wp.Web.Framework.Extensions.Mapper;

namespace Wp.Web.Api.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseCategoryController : ControllerBase
    {
        private readonly IExpenseCategoryService _expenseCategoryService;

        public ExpenseCategoryController(IExpenseCategoryService expenseCategoryService)
        {
            _expenseCategoryService = expenseCategoryService;
        }

        // GET: api/ExpenseCategory
        [HttpGet]
        public IActionResult Get()
        {
          var entities =  _expenseCategoryService.GetAll();
            var models = entities.ToModels();
            return Ok(models);
        }

        // GET: api/ExpenseCategory/5
        [HttpGet("{id}", Name = "GetExpenseCategory")]
        public IActionResult Get(int id)
        {
            var entity = _expenseCategoryService.GetById(id);
           var model = entity.ToModel();
            return Ok(model);

        }

        // POST: api/ExpenseCategory
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/ExpenseCategory/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
