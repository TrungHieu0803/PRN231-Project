using Microsoft.AspNetCore.Mvc;
using Client.Helper;
using System.Text.Json;
using Client.Models;

namespace Client.Controllers
{
    public class ProductController : Controller
    {
        private static CallAPI _callAPI = new CallAPI();
        public async Task<IActionResult> Index(int? id)
        {
            if (HttpContext.Session.GetString("Cart") != null)
            {
                List<OrderDetail> orders = Newtonsoft.Json.JsonConvert.DeserializeObject<List<OrderDetail>>(HttpContext.Session.GetString("Cart"));
                TempData["numberOfOrder"] = orders.Count;

            }
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                var account = Newtonsoft.Json.JsonConvert.DeserializeObject<AccountDto>(HttpContext.Session.GetString("UserSession"));
                TempData["User"] = account;
            }
            var product = await JsonSerializer.DeserializeAsync<ProductDto>(await _callAPI.Get($"http://localhost:5000/api/product/{id}", null));
            dynamic model = new System.Dynamic.ExpandoObject();
            model.product = product;
            return View(model);
        }
    }
}
