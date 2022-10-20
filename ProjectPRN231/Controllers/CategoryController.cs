using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectPRN231.Models;

namespace ProjectPRN231.Controllers
{
    [Route("api/category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private PRN231DBContext _context;
       
        public CategoryController(PRN231DBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Category>> Get()
        {
            var listCate = _context.Categories.ToList();
            return Ok(listCate);
        }
    }
}
