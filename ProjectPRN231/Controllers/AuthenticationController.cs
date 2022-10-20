using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProjectPRN231.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using ProjectPRN231.Dtos;
namespace ProjectPRN231.Controllers
{
    [Route("api")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private PRN231DBContext _context;
        IConfiguration _configuration;
        public AuthenticationController(PRN231DBContext context)
        {
            _context = context;
            _configuration = new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json")
                            .AddEnvironmentVariables()
                            .Build();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Post(LoginDto account)
        {
            if (account != null && account.Email != null && account.Password != null)
            {
                var acc = await _context.Accounts.FirstOrDefaultAsync(x => x.Email == account.Email && x.Password == account.Password);
                if (acc != null)
                {
                    // Create Claim details
                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("AccountId", acc.AccountId.ToString()),
                        new Claim("Password", acc.Password),
                        new Claim("Email", acc.Email),
                        new Claim("Password", acc.Password),
                        new Claim(ClaimTypes.Role, acc.Role.ToString())
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                            _configuration["Jwt:Issuer"],
                            _configuration["Jwt:Audience"],
                            claims,
                            expires: DateTime.UtcNow.AddMinutes(10),
                            signingCredentials: signIn
                        );
                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
                else
                {
                    return BadRequest("Invalid credentials");
                }
            }
            else
            {
                return BadRequest();
            }
            
        }
    }
}
