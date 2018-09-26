using System;
using System.DirectoryServices.AccountManagement;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using ITZone.API.Data;
using ITZone.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Internal;
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

        #region Active Directory
        //[HttpGet("ActiveDirectoryUser")]
        //public AD_User ActiveDirectoryUser(string strUserName, string strPassword)
        //{
        //    AD_User _ADUser = null;
        //  //  using (HostingEnvironment.Impersonate())
        //   // {
        //        var ADDomainName = _config["ADADomainName"];
        //    StreamWriter SW = new StreamWriter("C:\\Logs\\log.txt");
        //    SW.WriteLine(ADDomainName.ToString());
        //    SW.Close();
        //        //string ADDomainName = System.Configuration.ConfigurationManager.AppSettings["ADADomainName"];/*"LDAP://monshaat.gov.sa"*/
        //        var ctx = new PrincipalContext(ContextType.Domain, ADDomainName, strUserName, strPassword);
        //        string strDistinguishedName = "";
        //        bool bValid = ctx.ValidateCredentials(strUserName, strPassword);
        //        if (bValid)
        //        {
        //            UserPrincipal prUsr = new UserPrincipal(ctx);

        //            if (IsValidEmail(strUserName))
        //                prUsr.EmailAddress = strUserName;
        //            else
        //                prUsr.SamAccountName = strUserName;

        //            PrincipalSearcher srchUser = new PrincipalSearcher(prUsr);
        //            UserPrincipal foundUsr = srchUser.FindOne() as UserPrincipal;
        //            if (foundUsr != null)
        //            {
        //                _ADUser = new AD_User();
        //                strDistinguishedName = foundUsr.DistinguishedName;

        //                _ADUser.DisplayName = foundUsr.DisplayName;
        //                _ADUser.EmailAddress = foundUsr.EmailAddress;
        //                //_ADUser.ID = foundUsr.EmployeeId;
        //                _ADUser.SamAccountName = foundUsr.SamAccountName;
        //                _ADUser.Name = foundUsr.Name;
        //            }
        //            //else
        //            //    throw new AuthenticationException
        //            //    ("Please enter valid UserName/Password.");
        //        }
        //        //else
        //        //    throw new AuthenticationException
        //        //    ("Please enter valid UserName/Password.");

        //        return _ADUser;
        //   // }
        //}

        //bool IsValidEmail(string email)
        //{
        //    try
        //    {
        //        var addr = new System.Net.Mail.MailAddress(email);
        //        return addr.Address == email;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}
        #endregion

        #region Authentication / Token
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

        #endregion

        #region Gets
        [HttpGet("GetAll"), Authorize]
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

        [HttpGet("GetUserProfile"),Authorize]
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

        [HttpPut("UpdateUserPhoto"), Authorize]
        public IActionResult UpdateUserPhoto(photoVM user)
        {
            try
            {
                UserDB up = db.UserTokenDB.Find(user.ID);
                if (up != null)
                    up.photo = user.photo;
                db.SaveChanges();
                return NoContent();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("GetUsersBy"), Authorize]
        public IActionResult GetUsersBy(int userId)
        {
            try
            {
                var users = db.Assignees.FromSql($"select * from [itzonegl_MC].[Fn_GetAssignees]({userId})").ToList();
                return Ok(users);
            }
            catch
            {
                return BadRequest();
            }
        }
        #endregion

    }
}