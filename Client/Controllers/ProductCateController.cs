using Microsoft.AspNetCore.Mvc;
using Client.Helper;
using Client.Models;
namespace Client.Controllers
{
    public class ProductCateController : Controller
    {
        public async  Task<IActionResult> Index(string id)
        {
            var categories = await new CallAPI<IEnumerable<CategoryDto>>().Get("http://localhost:5000/api/category");
            var products = await new CallAPI<IEnumerable<ProductListDto>>().Get($"http://localhost:5000/api/product/category/{id}");
            dynamic model = new System.Dynamic.ExpandoObject();
            model.categories = categories;
            model.products = products;
            return View(model);
        }
    }
}
