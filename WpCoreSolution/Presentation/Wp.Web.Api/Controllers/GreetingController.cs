using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Wp.Web.Api.Controllers
{
    public class Country
    {
        public string Name { get; set; }
        public string Capital { get; set; }

    }
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class GreetingController : Controller
    {
        [HttpGet]
        public string Get()
        {
          return "Hello " + User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

        [HttpGet("autocomplete/{term}")]
        public List<Country> GetAutoComplete(string term)
        {
            List<Country> names = new List<Country>();

            for(int i = 0; i < 100; i++)
            {
                var newC = new Country();
                newC.Name = term + "Nam" + i.ToString();
                newC.Capital = term + "Capit" + i.ToString();
                names.Add(newC);
            }

            return names;
        }


        
    }
}
