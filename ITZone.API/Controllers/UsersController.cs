using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ITZone.API.Data;
using ITZone.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;


namespace ITZone.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IConfiguration _config;
        private Context db;
        public UsersController (IConfiguration config, Context context)
        {
            _config = config;
            db = context;
        }

        [AllowAnonymous]
        [HttpPost("Authenticate")]
        public IActionResult Authenticate([FromBody]LoginModel login)
        {
            IActionResult response = Unauthorized();
            var user = AuthenticateResult(login);

            if (user != null)
            {
                var tokenString = BuildToken(user);
                response = Ok(new { authenticated = "true", token = tokenString, userId = user.ID });
            }

            return response;
        }

        private UserDB AuthenticateResult(LoginModel login)
        {
            var HashedPassword = Security.Hashing.SHA256(login.Password.Trim());
            UserDB token = null;
            var user = db.UserTokenDB.Where(u => u.Name == login.Username && u.Password.Equals(HashedPassword)).FirstOrDefault();
            if (user != null)
            {
                token = new UserDB { ID = user.ID, Name = user.Name, Email = user.Email/*, Role = "Governor"*/};
            }
            return token;

        }

        private string BuildToken(UserDB user)
        {
            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, user.Name),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              claims,
              expires: DateTime.Now.AddMinutes(30), //(Double)_config["Jwt:SessionTime"]
              signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpGet("GetAll"),Authorize]
        public IActionResult GetALL()
        {
            try
            {
                return Ok(db.Users.ToList());
            }
            catch
            {
                return BadRequest();
            }
            
        }
        [HttpGet("GetUserProfile")]
        public IActionResult GetUserProfile(int userId)
        {
            try
            {
                var user = db.Users.FromSql($"SELECT * FROM [Fn_GetUserProfile] ({userId})").FirstOrDefault();
                return Ok(user);
            }
            catch
            {
                return BadRequest();
            }

        }

        [HttpPut("UpdateUserPhoto")]
        public IActionResult UpdateUserPhoto(photoVM user)
        {
            UserDB up = db.UserTokenDB.Find(user.ID);
            //var up = db.Users.FromSql($"SELECT * FROM [Fn_GetUserProfile] ({user.ID})").FirstOrDefault();

            if (up != null)
                up.photo = user.photo;
            db.SaveChanges();
            return NoContent();
        }
        [HttpGet("GetUsersBy")]
        public IActionResult GetUsersBy(int userId)
        {
            var users = db.Assignees.FromSql($"select * from [itzonegl_MC].[Fn_GetAssignees]({userId})").ToList();
            return Ok(users);
        }
        [HttpGet("Get"), Authorize]
        public IActionResult Get()
        {
            return Ok("Karim, kali@itzoneglobal.com, Governor");
        }
    }
}