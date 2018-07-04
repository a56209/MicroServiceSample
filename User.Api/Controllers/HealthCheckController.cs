using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using User.Api.Data;
using Microsoft.AspNetCore.JsonPatch;

namespace User.Api.Controllers
{    
    [Route("[Controller]")]
    public class HealthCheckController : Controller
    {     
        [HttpGet("")]
        [HttpHead("")]
        public IActionResult Ping()
        {
            return Ok();
        }       
    }
}