using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITZone.API.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITZone.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PriorityController : ControllerBase
    {
        private Context db;

        public PriorityController(Context context)
        {
            db = context;
        }

        [HttpGet("GetPriority")]
        public IActionResult Getpriorities()
        {
            try
            {
                var priorities = db.Priorities.FromSql("SELECT * FROM [Fn_GetPriority] ()").ToList();
                return Ok(priorities);
            }
            catch
            {
                return BadRequest();
            }
        }

    }
}