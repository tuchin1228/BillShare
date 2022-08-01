using BillShare.Data;
using BillShare.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BillShare.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        public LoginController(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult Login(LoginDTO userObj)
        {

            var user = _context.Users.FirstOrDefault(m => m.UserId == userObj.UserId && m.Password == userObj.Password);

            if (user == null)
            {
                return BadRequest("帳號密碼錯誤");
            }
            else
            {
                var claims = new List<Claim>
                {
                    new Claim("UserId", user.UserId),
                    //new Claim("FullName", user.Name),
                    //new Claim(JwtRegisteredClaimNames.NameId, user.EmployeeId.ToString())
                };

                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:KEY"]));

                var jwt = new JwtSecurityToken
                    (
                        issuer: _configuration["JWT:Issuer"],
                        claims: claims,
                        expires: DateTime.Now.AddMinutes(60),
                        signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
                    );

                var token = new JwtSecurityTokenHandler().WriteToken(jwt);
                return Ok(new
                {
                    success = true,
                    token = token,
                });
            }


        }
    }
}
