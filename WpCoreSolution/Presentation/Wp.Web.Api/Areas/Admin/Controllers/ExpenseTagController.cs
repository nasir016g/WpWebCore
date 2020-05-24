using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Wp.Services.Expenses;
using Wp.Web.Api.Extensions.Mapper;
using Wp.Web.Api.Models.Admin;

namespace Wp.Web.Api.Areas.Admin.Controllers
{
    [Route("api/admin/[controller]")]
    [ApiController]
    public class ExpenseTagController : ControllerBase
    {
        private readonly IExpenseTagService _expenseTagService;

        public ExpenseTagController(IExpenseTagService expenseTagService)
        {
            _expenseTagService = expenseTagService;
        }



        // GET: api/ExpenseTag
        [HttpGet]
        public ObjectResult Get()
        {
            var entities = _expenseTagService.GetAll();
            var models = entities.ToModels();
            return Ok(models);
        }

        // GET: api/ExpenseTag/5
        [HttpGet("{id}", Name = "GetExpenseTag")]
        public IActionResult Get(int id)
        {
            var entity = _expenseTagService.GetById(id);
            var model = entity.ToModel();

            return Ok(model);
        }

        // POST: api/ExpenseTag
        [HttpPost]
        public IActionResult Post([FromBody] ExpenseTagModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var entity = model.ToEntity();
            entity.Id = 0;
            _expenseTagService.Insert(entity);
            return NoContent();
        }

        // PUT: api/ExpenseTag/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] ExpenseTagModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var entity = _expenseTagService.GetById(id);
            entity = model.ToEntity(entity);
            _expenseTagService.Update(entity);
            return NoContent();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var entity = _expenseTagService.GetById(id);
            _expenseTagService.Delete(entity);
        }
    }
}
