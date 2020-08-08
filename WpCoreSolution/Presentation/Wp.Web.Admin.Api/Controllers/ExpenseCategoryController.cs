using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Wp.Services.Expenses;
using Wp.Web.Admin.Api.Extensions.Mapper;

namespace Wp.Web.Admin.Api.Controllers
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
        public string Get(int id)
        {
            return "value";
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
