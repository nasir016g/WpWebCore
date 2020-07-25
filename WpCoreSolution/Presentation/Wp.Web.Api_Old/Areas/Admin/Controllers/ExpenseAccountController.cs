using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Wp.Core.Domain.Expenses;
using Wp.Services.Expenses;
using Wp.Web.Api.Extensions.Mapper;

namespace Wp.Web.Api.Areas.Admin.Controllers
{
    [Route("api/admin/[controller]")]
    [ApiController]
    public class ExpenseAccountController : ControllerBase
    {
        private readonly IExpenseAccountService _expenseAccountService;

        public ExpenseAccountController(IExpenseAccountService expenseAccountService)
        {
            _expenseAccountService = expenseAccountService;
        }

        // GET: api/ExpenseAccount
        [HttpGet]
        public IActionResult Get()
        {
            var entities = _expenseAccountService.GetAll();
            var models = entities.ToModels();
            return Ok(models);
        }

        // GET: api/ExpenseAccount/5
        [HttpGet("{id}", Name = "GetExpenseAccount")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/ExpenseAccount
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/ExpenseAccount/5
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
