using Client.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;
using Client.Helper;
namespace Client.Controllers
{
    public class HomeController : Controller
    {
        
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await new CallAPI<IEnumerable<CategoryDto>>().Get("http://localhost:5000/api/category");
            var bestSaleProduct = await new CallAPI<IEnumerable<ProductListDto>>().Get("http://localhost:5000/api/product/best-sale");
            var newProduct = await new CallAPI<IEnumerable<ProductListDto>>().Get("http://localhost:5000/api/product/new");
            dynamic model = new System.Dynamic.ExpandoObject();
            model.categories = categories;
            model.bestSaleProduct = bestSaleProduct;  
            model.newProduct = newProduct;
            return View(model);
        }

        public IActionResult SignIn()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}