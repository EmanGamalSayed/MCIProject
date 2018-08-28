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
    public class StatusController : ControllerBase
    {
        private Context db;

        public StatusController(Context context)
        {
            db = context;
        }

        [HttpGet("GetStatus")]
        public IActionResult GetStatus()
        {
            try
            {
                var status = db.Status.FromSql("SELECT * FROM [Fn_GetStatus] ()").ToList();
                return Ok(status);
            }
            catch
            {
                return BadRequest();
            }


        }
    }
}