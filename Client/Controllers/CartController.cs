using Microsoft.AspNetCore.Mvc;
using Client.Models;
using System.Text.Json;
using Client.Helper;

namespace Client.Controllers
{
    public class CartController : Controller
    {
        private static CallAPI _callAPI = new CallAPI();

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("Cart") != null)
            {
                List<OrderDetail> orders = Newtonsoft.Json.JsonConvert.DeserializeObject<List<OrderDetail>>(HttpContext.Session.GetString("Cart"));
                TempData["numberOfOrder"] = orders.Count;
                dynamic model = new System.Dynamic.ExpandoObject();
                model.orderDetails = orders;
                if (HttpContext.Session.GetString("UserSession") != null)
                {
                    var token = HttpContext.Session.GetString("token");
                    var account = Newtonsoft.Json.JsonConvert.DeserializeObject<AccountDto>(HttpContext.Session.GetString("UserSession"));
                    var customer = await JsonSerializer.DeserializeAsync<CustomerDto>(await _callAPI.Get($"http://localhost:5000/api/customer/{account.accountId}", token));
                    model.customer = customer;
                    TempData["User"] = account;
                }
                return View(model);
            }

            return View();
        }


        public IActionResult AddToCart(int id, string name, decimal price)
        {
            if (HttpContext.Session.GetString("Cart") == null)
            {
                List<OrderDetail> orders = new List<OrderDetail>();
                orders.Add(new OrderDetail { productId = id, quantity = 1, productName = name, unitPrice = price });
                HttpContext.Session.SetString("Cart", Newtonsoft.Json.JsonConvert.SerializeObject(orders));
                TempData["numberOfOrder"] = 1;
            }
            else
            {
                List<OrderDetail> orders = Newtonsoft.Json.JsonConvert.DeserializeObject<List<OrderDetail>>(HttpContext.Session.GetString("Cart"));
                HttpContext.Session.Remove("Cart");

                var o = orders.Where(x => x.productId == id).SingleOrDefault();
                if (o == null)
                {
                    orders.Add(new OrderDetail { productId = id, quantity = 1, productName = name, unitPrice = price });
                } 
                else
                {
                    foreach (var order in orders)
                    {
                        if(order.productId == id)
                        {
                            order.quantity += 1;
                        }
                    }
                }

                HttpContext.Session.SetString("Cart", Newtonsoft.Json.JsonConvert.SerializeObject(orders));
                TempData["numberOfOrder"] = orders.Count;
            }
            
            return RedirectToAction("Index");
        }

        public IActionResult Increase(int id)
        {
            List<OrderDetail> orders = Newtonsoft.Json.JsonConvert.DeserializeObject<List<OrderDetail>>(HttpContext.Session.GetString("Cart"));
            foreach (var order in orders)
            {
                if (order.productId == id)
                {
                    order.quantity += 1;
                }
            }
            HttpContext.Session.SetString("Cart", Newtonsoft.Json.JsonConvert.SerializeObject(orders));
            return RedirectToAction("Index");
        }

        public IActionResult Decrease(int id)
        {
            List<OrderDetail> orders = Newtonsoft.Json.JsonConvert.DeserializeObject<List<OrderDetail>>(HttpContext.Session.GetString("Cart"));
            foreach (var order in orders)
            {
                if (order.productId == id && order.quantity > 1)
                {
                    order.quantity -= 1;
                }
            }
            HttpContext.Session.SetString("Cart", Newtonsoft.Json.JsonConvert.SerializeObject(orders));
            return RedirectToAction("Index");
        }
        public IActionResult Remove(int id)
        {
            List<OrderDetail> orders = Newtonsoft.Json.JsonConvert.DeserializeObject<List<OrderDetail>>(HttpContext.Session.GetString("Cart"));
            foreach (var order in orders)
            {
                if (order.productId == id && order.quantity > 1)
                {
                    orders.Remove(order);
                    break;
                }
            }
            HttpContext.Session.SetString("Cart", Newtonsoft.Json.JsonConvert.SerializeObject(orders));
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> CreateOrder(string shipAdress, string customerId)
        {
            if (HttpContext.Session.GetString("Cart") == null)
            {
                
                return View();
            } else
            {
                List<OrderDetail> orderDetail = Newtonsoft.Json.JsonConvert.DeserializeObject<List<OrderDetail>>(HttpContext.Session.GetString("Cart"));
                Order order = new Order { customerId = customerId, shipAddress = shipAdress, orderDetail = orderDetail };
                var orderResponse = await _callAPI.Post("http://localhost:5000/api/order", order);
                return View();

            }
        }

    }



}
