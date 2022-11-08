using Microsoft.AspNetCore.Mvc;
using Client.Helper;
using Client.Models;
using System.Text.Json;

namespace Client.Controllers
{
    public class ProductCateController : Controller
    {

        private static CallAPI callAPI = new CallAPI();

        public async  Task<IActionResult> Index(string id)
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                var account = Newtonsoft.Json.JsonConvert.DeserializeObject<AccountDto>(HttpContext.Session.GetString("UserSession"));
                TempData["User"] = account;
            }
            var categories = await JsonSerializer.DeserializeAsync<IEnumerable<CategoryDto>>(await callAPI.Get("http://localhost:5000/api/category", null));
            var products = await JsonSerializer.DeserializeAsync<IEnumerable<ProductDto>>(await callAPI.Get($"http://localhost:5000/api/product/category/{id}", null));
            dynamic model = new System.Dynamic.ExpandoObject();
            model.categories = categories;
            model.products = products;
            return View(model);
        }
    }
}
