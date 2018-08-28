using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITZone.API.Data;
using ITZone.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITZone.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SectorsController : ControllerBase
    {
        private Context db;

        public SectorsController(Context context)
        {
            db = context;

        }
        [HttpGet("GetSectors")]
        public IActionResult GetSectors()
        {
            try
            {
                var sectors = db.Sectors.FromSql("SELECT * FROM [Fn_GetSectors] ()").ToList();
                return Ok(sectors);
            }
            catch
            {
                return BadRequest();
            }
            

        }
    }
}