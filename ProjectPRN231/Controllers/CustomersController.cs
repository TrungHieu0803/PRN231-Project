using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectPRN231.Models;
using Microsoft.EntityFrameworkCore;
namespace ProjectPRN231.Controllers
{
    [Route("api/customer")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private PRN231DBContext _context;

        public CustomersController(PRN231DBContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "2")]
        public ActionResult<IEnumerable<Customer>> Get()
        {
            return Ok(_context.Customers.ToList());
        }

        [HttpGet]
        [Authorize(Roles = "2")]
        [Route("{id}")]
        public ActionResult<IEnumerable<Customer>> GetOne(int id)
        {
            try
            {
                var account = _context.Accounts.Where(x => x.AccountId == id).Include(x => x.Customer).FirstOrDefault();
               
                if(account == null)
                {
                    return NotFound();
                }
                return Ok(account.Customer);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message); 
            }
           
        }

        [HttpPost] 
        [Authorize(Roles = "2")]
        public ActionResult Post([FromBody]Account account)
        {
            _context.Accounts.Add(account);
            _context.SaveChanges();
            return Ok();
        }


    }
}
