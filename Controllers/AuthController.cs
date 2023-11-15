using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ShowroomManagement.Data;
using ShowroomManagement.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ShowroomManagement.Controllers
{
    [DisableCors]
    public class AuthController : Controller
    {
        private IConfiguration _config;
        private ShowroomContext db;

        public AuthController(IConfiguration config, ShowroomContext context)
        {
            _config = config;
            db = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public Task<IActionResult> Login([FromBody] Account account)
        {
            var username = account.Username;
            var password = account.Password;

            Account user = Authenticate(
                new Account()
                {
                    Username = username,
                    Password = password
                });

            if (user != null)
            {
                var token = Generate(user);
                return Task.FromResult<IActionResult>(Ok(new { access_token = token }));
            }
            else
            {
                return Task.FromResult<IActionResult>(Ok(new { error = "Username hoặc Password không đúng." }));
            }
        }

        public IActionResult Logout()
        {
            // 
            return Ok();
        }

        public Task<IActionResult> LoginForm([FromForm] Account account)
        {
            var username = account.Username;
            var password = account.Password;

            Account user = Authenticate(new Account() { Username = username, Password = password });

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

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
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
