using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectPRN231.Dtos;
using ProjectPRN231.Models;
using System.Security.Claims;

namespace ProjectPRN231.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private PRN231DBContext _context;
        public readonly IMapper mapper;
        public OrderController(PRN231DBContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "1")]
        public async Task<ActionResult<IEnumerable<Product>>> Get([FromForm] CreateOrderDto order)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            int userId = Int32.Parse(identity.FindFirst("AccountId").Value);
            try
            {
                Account account = await _context.Accounts.Where(a => a.AccountId == userId).FirstOrDefaultAsync();
                if (account == null)
                {
                    return BadRequest();
                }
                else
                {
                    return Ok();
                }
                
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }



        }

        [HttpPost]
        [Authorize(Roles = "2")]
        public async Task<ActionResult<IEnumerable<Product>>> CreateOrder([FromBody] CreateOrderDto order)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            int userId = Int32.Parse(identity.FindFirst("AccountId").Value);
            try
            {
                Account account = await _context.Accounts.Where(a => a.AccountId == userId).FirstOrDefaultAsync();
                if (account == null)
                {
                    return BadRequest();
                }
                else
                {
                    return Ok();
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }



        }

    }
}
