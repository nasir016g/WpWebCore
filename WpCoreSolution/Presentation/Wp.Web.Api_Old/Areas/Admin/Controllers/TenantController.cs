using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Wp.Core;
using Wp.Web.Api.Extensions.Mapper;
using Wp.Web.Api.Models.Admin;

namespace Wp.Web.Api.Areas.Admin.Controllers
{
    [Route("api/admin/[controller]")]
    [ApiController]
    public class TenantController : ControllerBase
    {
        private readonly ITenantService tenantService;

        public TenantController(ITenantService tenantService)
        {
            this.tenantService = tenantService;
        }
        // GET: api/Tenant
        [HttpGet]
        public ObjectResult Get()
        {
           var entities = this.tenantService.GetAll();
            var models = entities.ToModels();
            return Ok(models);
        }

        // GET: api/Tenant/5
        [HttpGet("{id}", Name = "GetById")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Tenant
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Tenant/5
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
