using CoreCodeFirst.Data;
using CoreCodeFirst.Models;
using CoreCodeFirst.Repository.IRepository;
using CoreCodeFirst.Utilities;
using CoreCodeFirst.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Stripe;
using Stripe.Checkout;
using Microsoft.AspNetCore.Http;
//using Stripe.BillingPortal;

namespace CoreCodeFirst.Areas.Customer.Controllers
{
    public class LoginAndRegister : Controller
    {
        public readonly DatabaseContext _db;
       string UserType = "";
    //    HttpContext.Session.SetString(UserType, "customer");
        /*      public IActionResult Index()
              {
                  HttpContext.Session.SetString(UserType, "Jarvik");
                  HttpContext.Session.SetInt32(SessionAge, 24);
                  return View();
              }*/
        public LoginAndRegister(DatabaseContext db)
        {
            // HttpContext.Session.SetString(UserType, "customer");
            // TempData["userType"] = HttpContext.Session.GetString(UserType);
           // HttpContext.Session.SetString("UserType", "customer");
           // HttpContext.Session.SetInt32("Age", 30);
            // Get value from Session.
           // string user = HttpContext.Session.GetString("UserType");
           // int? age = HttpContext.Session.GetInt32("Age");
           // ViewBag.Message = "Usertype : " + user ;

            ViewBag.UserType = "customer";
            _db = db;
  
        }
        public IActionResult LoginUser()
        {
            return View();
        }
        public IActionResult LogOut()
        {
            try
            {
                TempData["userType"] = "";
                ViewBag.UserType = "";
                HttpContext.Session.SetString("UserType", "");
                HttpContext.Session.SetString("LogStatus", "LoggedOut");
                return RedirectToAction("Index", "Home");

            }
            catch (Exception ex)
            {

                return RedirectToAction("ErrorIndex", "Error", new { area = "Customer" });
            }
         
        }
        [HttpPost]
        public IActionResult LoginUser(userLogin user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user1 = (from e in _db.User
                                 where e.Email == user.Email & e.Password == user.Password
                                 select e).FirstOrDefault();
                    //if (user.UserName.Equals(_db.User.UserName))
                    // Product productModel = new Product();
                    if (user1 == null)
                    {

                        TempData["error"] = "user not exist..!";
                        Console.WriteLine("logged failed");
                        return RedirectToAction("LoginUser");
                        //return RedirectToAction("LoginUser");
                    }
                    else
                    {
                        if (user1.UserTypeId.ToString().ToLower() == "1")
                        {
                            HttpContext.Session.SetString("UserType", "admin");
                            // HttpContext.Session.SetInt32("Age", 30);
                            // Get value from Session.
                            ViewBag.Message = HttpContext.Session.GetString("UserType");
                            // int? age = HttpContext.Session.GetInt32("Age");
                            //ViewBag.Message = "Usertype : " + user;
                            //ViewBag.UserType = "admin";
                            // HttpContext.Session.SetString(UserType,"admin");
                            //TempData["userType"] = "admin";

                            //TempData["userType"] = "admin";
                        }
                        else
                        {
                            //ViewBag.UserType = "admin";
                            HttpContext.Session.SetString("UserType", "customer");
                            ViewBag.Message = HttpContext.Session.GetString("UserType");
                            //TempData["userType"] = HttpContext.Session.GetString(UserType);
                            //TempData["userType"] = "customer";
                        }
                        HttpContext.Session.SetInt32("UserId", user1.Id);
                        HttpContext.Session.SetString("LogStatus", "LoggedIn");
                        TempData["success"] = "logged sucessfully...";
                        Console.WriteLine("logged succ");
                        return RedirectToAction("Index", "Home");
                    }
                }
                return View();

            }
            catch (Exception ex)
            {

                return RedirectToAction("ErrorIndex", "Error", new { area = "Customer" });
            }
         
        }
        public IActionResult RegisterUser()
        {
            try
            {
                IEnumerable<SelectListItem> UserList = _db.UserType.Select(c => new SelectListItem()
                {
                    Text = c.Users,
                    Value = c.Id.ToString()
                });
                // IEnumerable<Product> ProductList = _db.Products;
                return View();

            }
            catch (Exception ex)
            {

                return RedirectToAction("ErrorIndex", "Error", new { area = "Customer" });
            }
           
        }
        [HttpPost]
        public IActionResult RegisterUser(User user)
        {
            try
            {

                IEnumerable<SelectListItem> UserList = _db.UserType.Select(c => new SelectListItem()
                {
                    Text = c.Users,
                    Value = c.Id.ToString()
                });
                if (ModelState.IsValid)
                {
                    var user1 = (from e in _db.User
                                 where e.Email == user.Email
                                 select e).FirstOrDefault();
                    //if (user.UserName.Equals(_db.User.UserName))
                    // Product productModel = new Product();
                    if (user1 == null)
                    {
                        user.CreatedDate = DateTime.Now;
                        user.ModifiedDate = DateTime.Now;
                        user.UserTypeId = 2;
                        _db.User.Add(user);
                        _db.SaveChanges();
                        TempData["success"] = "Registered successfully..!";
                        return RedirectToAction("LoginUser");
                    }
                    else
                    {
                        TempData["error"] = "Email already exists..!";
                        return RedirectToAction("ResgisterUser");
                    }
                }
                return View();
            }
            catch (Exception ex)
            {

                return RedirectToAction("ErrorIndex", "Error", new { area = "Customer" });
            }
          
        }

    }
}
