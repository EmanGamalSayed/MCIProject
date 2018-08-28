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
    public class DepartmentController : ControllerBase
    {
        private Context db;

        public DepartmentController(Context context)
        {
            db = context;
        }

        [HttpGet("GetDepartment")]
        public IActionResult GetDepartment()
        {
            try
            {
                var departments = db.Departments.FromSql("SELECT * FROM [Fn_GetDepartment] ()").ToList();
                return Ok(departments);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}