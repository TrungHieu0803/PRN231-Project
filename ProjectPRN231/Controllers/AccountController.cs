using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectPRN231.Models;
using ProjectPRN231.Dtos;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace ProjectPRN231.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private PRN231DBContext _context;
        
        public AccountController(PRN231DBContext context)
        {
            _context = context;
           
        }

        [HttpGet]
        [Authorize(Roles = "1,2")]
        public async Task<ActionResult<Account>> GetUser([FromQuery]string email)
        {
            var account = await _context.Accounts.Where(x => x.Email == email).Select(x => new {x.AccountId, x.Role}).FirstOrDefaultAsync();
            if (account == null)
            {
                return NotFound();
            }

            return Ok(account);
        }
    }
}
