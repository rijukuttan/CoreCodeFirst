using CoreCodeFirst.Models;
using CoreCodeFirst.Repository.IRepository;
using CoreCodeFirst.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CoreCodeFirst.Controllers
{
    public class HomeController : Controller
    {
       // TempData["userType"] = "customer";
        private readonly ILogger<HomeController> _logger;
        public readonly IUnitOfWorkRepository _unitOfWork;
        public HomeController(ILogger<HomeController> logger, IUnitOfWorkRepository db)
        {
           // TempData["userType"] = "customer";
            _logger = logger;
            _unitOfWork = db;
        
        }
        public IActionResult Index()
        {
            try
            {
                /*HttpContext.Session.SetString("Name", "Dharmendra");
                       HttpContext.Session.SetInt32("Age", 30);
                       // Get value from Session.
                       string name = HttpContext.Session.GetString("Name");
                       int? age = HttpContext.Session.GetInt32("Age");
                       ViewBag.Message = "Name : " + name + "<br />Age : " + age;*/
                IEnumerable<Product> productList = _unitOfWork.ProductRep.GetAll();
                return View(productList);

            }
            catch (Exception ex)
            {

                return RedirectToAction("ErrorIndex", "Error", new { area = "Customer" });
            }
       
        }
        public IActionResult Details(int productId)
        {
            try
            {
                ShoppingCart shoppingCart = new()
                {
                    ProductId = productId,
                    Product = _unitOfWork.ProductRep.GetFirstOrDefault(p => p.Id == productId),
                    Count = 1
                };
                return View(shoppingCart);
            }
            catch (Exception ex)
            {

                return RedirectToAction("ErrorIndex", "Error", new { area = "Customer" });
            }
         
        }
        [HttpPost]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            try
            {
                if (HttpContext.Session.GetString("LogStatus") == "LoggedIn")
                {
                    int id = (int)HttpContext.Session.GetInt32("UserId");
                    ShoppingCart cart = _unitOfWork.ShoppingCartRep.GetFirstOrDefault(
                    u => u.ProductId == shoppingCart.ProductId & u.UserId == id);
                    if (cart == null)
                    {

                        shoppingCart.UserId = id;
                        _unitOfWork.ShoppingCartRep.Add(shoppingCart);
                        _unitOfWork.Save();
                        // HttpContext.Session.SetInt32(AppConstants.SessionCart, _unitOfWork.ShoppingCartRep.GetAll().ToList().Count);
                    }
                    else
                    {
                        _unitOfWork.ShoppingCartRep.IncrementCount(cart, shoppingCart.Count);
                        _unitOfWork.Save();
                    }
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return RedirectToAction("LoginUser", "LoginAndRegister");
                }

            }
            catch (Exception ex)
            {

                return RedirectToAction("ErrorIndex", "Error", new { area = "Customer" });
            }
          
      
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}