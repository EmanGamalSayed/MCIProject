using ITZone.API.Data;
using ITZone.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace ITZone.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private IConfiguration _config;
        private Context _context;
            
        public TokenController(IConfiguration config, Context context)
        {
            _config = config;
            _context = context;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult CreateToken([FromBody]LoginModel login)
        {
            IActionResult response = Unauthorized();
            var user = AuthenticateResult(login);

            if (user != null)
            {
                var tokenString = BuildToken(user);
                response = Ok(new { token = tokenString });
            }

            return response;
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
              expires: DateTime.Now.AddMinutes(30),
              signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        
        private UserDB AuthenticateResult(LoginModel login)
        {
            var HashedPassword = Security.Hashing.SHA256(login.Password.Trim());
            UserDB userToken = null;
            var user = _context.UserTokenDB.Where(u => u.Name == login.Username && u.Password.Equals(HashedPassword)).FirstOrDefault();
            if (user != null)
            {
                userToken = new UserDB { ID = user.ID, Name = user.Name, Email = user.Email/*, Role = "Governor"*/};
            }
            return userToken;

        }

        //public TokenController(Context context)
        //{
        //    _context = context;
        //}

        

        //private UserModel Authenticate(LoginModel login)
        //{
        //    UserModel user = null;

        //    if (login.Username == "karim" && login.Password == "password")
        //    {
        //        user = new UserModel { Name = "Mario Rossi", Email = "mario.rossi@domain.com"/*, Role = "Governor"*/};
        //    }
        //    return user;
        //}

    }
}