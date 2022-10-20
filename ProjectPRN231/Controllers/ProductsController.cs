using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectPRN231.Models;
using ProjectPRN231.Dtos;
using Microsoft.EntityFrameworkCore;
namespace ProjectPRN231.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private PRN231DBContext _context;
        public readonly IMapper mapper;
        public ProductsController(PRN231DBContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "2")]
        public ActionResult<IEnumerable<Product>> Get([FromQuery] int? categoryId, [FromQuery] string? searchString)
        {
            if (searchString != null)
            {
                if (categoryId != null)
                {
                    var c = _context.Products.Where(p => p.ProductName.ToLower().Contains(searchString.ToLower()) && p.CategoryId == categoryId).ToList();
                    return Ok(c);
                }
                var customers = _context.Products.Where(p => p.ProductName.ToLower().Contains(searchString.ToLower())).ToList();
                return Ok(customers);
            }
            else
            {
                var customers = _context.Products.ToList();
                return Ok(customers);
            }
        }

        [HttpGet]
        [Route("hot")]
        public ActionResult<IEnumerable<Product>> GetListProduct()
        {
            return null;
        }

        [HttpGet]
        [Route("best-sale")]
        public async Task<ActionResult<IEnumerable<Product>>> GetBestSale()
        {

            //var products = await _context.Products
            //    .FromSqlRaw("select [Order Details].ProductID, Products.ProductName, Products.CategoryID, Products.Discontinued, COUNT(*) as Number " +
            //    "from Products " +
            //    "inner join [Order Details] on Products.ProductID = [Order Details].ProductID " +
            //    "group by [Order Details].ProductID, Products.ProductName, Products.CategoryID, Products.Discontinued " +
            //    "order by Number desc").ToListAsync();
            try
            {
                var product = await (from p in _context.Products
                               join o in _context.OrderDetails on p.ProductId equals o.ProductId
                               group p by p.ProductId into o
                               orderby o.Count() descending
                               select o.Select(x => new { x.ProductId, x.ProductName, x.UnitPrice, number = o.Count() }).First()).Take(4).ToListAsync();
                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }

        }

        [HttpGet]
        [Route("new")]
        public async Task<ActionResult<IEnumerable<Product>>> GetNew()
        {

            try
            {
                var product = await _context.Products.OrderByDescending(x => x.ProductId).Take(4).ToListAsync();
                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }

        }

        [HttpGet]
        [Route("category/{id}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductCategory(int id)
        {

            try
            {
                var product = await _context.Products.Where(x => x.CategoryId == id).Take(12).ToListAsync();
                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }

        }

        [HttpPost]
        [Authorize(Roles = "2")]
        public ActionResult Post([FromBody] ProductDto productDto)
        {

            var product = mapper.Map<Product>(productDto);
            var category = _context.Categories.Where(c => c.CategoryId == productDto.CategoryId).FirstOrDefault();
            if (category == null)
            {
                return BadRequest("categoryId invalid");
            }
            else
            {
                _context.Products.Add(product);
                _context.SaveChanges();
                return Ok();
            }
        }
    }
}
