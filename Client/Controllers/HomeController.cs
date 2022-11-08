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
        private static CallAPI _callAPI = new CallAPI();
       
       
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            

            
        }

        public async Task<IActionResult> Index()
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
            var categories = await JsonSerializer.DeserializeAsync<IEnumerable<CategoryDto>>(await _callAPI.Get("http://localhost:5000/api/category", null));
            var bestSaleProduct = await JsonSerializer.DeserializeAsync<IEnumerable<ProductDto>>(await _callAPI.Get("http://localhost:5000/api/product/best-sale", null));
            var newProduct = await JsonSerializer.DeserializeAsync<IEnumerable<ProductDto>>(await _callAPI.Get("http://localhost:5000/api/product/new", null));
            
            dynamic model = new System.Dynamic.ExpandoObject();
            model.categories = categories;
            model.bestSaleProduct = bestSaleProduct;  
            model.newProduct = newProduct;

            return View(model);
        }

        public async Task<IActionResult> SignIn(string? email, string? password)
        {
            if (!string.IsNullOrEmpty(email))
            {
                
                
                var loginResponse = await _callAPI.Post("http://localhost:5000/api/login", new { email = email, password = password });
                
                if (loginResponse.IsSuccessStatusCode)
                {
                    var token = await JsonSerializer.DeserializeAsync<TokenDto>(loginResponse.Content.ReadAsStream());
                   
                    HttpContext.Session.SetString("token", token.token);
                    await JsonSerializer.DeserializeAsync<AccountDto>(await _callAPI.Get($"http://localhost:5000/api/account?email={email}", token.token));
                    var s  = await JsonSerializer.DeserializeAsync<AccountDto>(await _callAPI.Get($"http://localhost:5000/api/account?email={email}", token.token));
                    HttpContext.Session.SetString("UserSession", Newtonsoft.Json.JsonConvert.SerializeObject(s));
                    if (s.role == 2)
                    {
                        return RedirectToAction("Index");
                    }else
                    {
                        return View();
                    }
                    
                }
               
                
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}