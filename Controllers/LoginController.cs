using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ShowroomManagement.Data;
using ShowroomManagement.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;

namespace ShowroomManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IConfiguration _config;
        private ShowroomContext db;

        public LoginController(IConfiguration config, ShowroomContext context)
        {
            _config = config;
            db = context;
        }

        [AllowAnonymous]
        public Task<IActionResult> Login([Bind("username, password")]Account account)
        {
            Account user = Authenticate(new Account() { Username = account.Username, Password = account.Password });

            if (user != null)
            {
                var token = Generate(user);
                return Task.FromResult<IActionResult>(Ok(token));
            }

            return Task.FromResult<IActionResult>(NotFound("User not found"));
        }

        private string Generate(Account account)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var crendentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, account.Username),
                new Claim(ClaimTypes.Role, Convert.ToString(account.Level_account))
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: crendentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private Account Authenticate(Account account)
        {
            // TODO: authenticate the account
            if (db.Accounts == null) return null;

            var query = db.Accounts.FromSqlRaw("SELECT * FROM DBO.LOGIN_CHECK(@username, @password)",
                 new SqlParameter("@username", account.Username),
                 new SqlParameter("@password", account.Password)).Select(p => new Account()
                 {
                     Username = p.Username,
                     Password = p.Password,
                     Level_account = p.Level_account,
                 }).FirstOrDefault();

            return query;
        }
    }
}
