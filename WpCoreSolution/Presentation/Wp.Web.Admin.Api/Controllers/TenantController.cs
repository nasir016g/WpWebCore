﻿using Microsoft.AspNetCore.Mvc;
using Wp.Core;
using Wp.Web.Framework.Extensions.Mapper;

namespace Wp.Web.Api.Admin.Controllers
{
    [Route("api/[controller]")]
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
