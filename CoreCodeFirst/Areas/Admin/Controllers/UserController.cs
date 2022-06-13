using CoreCodeFirst.Data;
using CoreCodeFirst.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CoreCodeFirst.Controllers
{
    public class UserController : Controller
    {
       // private readonly DatabaseContext _db;
        internal DbSet<User> dbset;
    /*    public Repository(DatabaseContext db)
        {
            _db = db;
            this.dbset = _db.Set<T>();
        }*/
        public readonly DatabaseContext _db;
        public UserController(DatabaseContext db)
        {
          
            _db = db;
            this.dbset = _db.Set<User>();
        }
       /* public IEnumerable<User> GetUserList(Expression<Func<User, bool>>? filter = null, string? includeProperties = null)
        {
            try
            {
                IQueryable<User> query = dbset;
                if (filter != null)
                {
                    query = query.Where(filter);
                }
                if (includeProperties != null)
                {
                    foreach (var property in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        query = query.Include(property);

                    }
                }
                return query.ToList();

            }
            catch (Exception ex)
            {

                return RedirectToAction("ErrorIndex", "Error", new { area = "Customer" });
            }
           
        }*/
        public IActionResult Index()
        {
            try
            {
                /*IEnumerable<User> UserList = GetUserList(includeProperties: "UserType");
                return View(UserList);*/
                  IEnumerable<User> UserList = _db.User;
                  return View(UserList);

            }
            catch (Exception ex)
            {

                return RedirectToAction("ErrorIndex", "Error", new { area = "Customer" });
            }
         
        }
        public IActionResult CreateUser()
        {
            try
            {
                IEnumerable<SelectListItem> UserList = _db.UserType.Select(c => new SelectListItem()
                {
                    Text = c.Users,
                    Value = c.Id.ToString()
                });
                ViewBag.UserList = UserList;
                // IEnumerable<Product> ProductList = _db.Products;
                return View();

            }
            catch (Exception ex)
            {

                return RedirectToAction("ErrorIndex", "Error", new { area = "Customer" });
            }
         
        }
        [HttpPost]
        public IActionResult CreateUser(User user)
        {
            try
            {
                IEnumerable<SelectListItem> UserList = _db.UserType.Select(c => new SelectListItem()
                {
                    Text = c.Users,
                    Value = c.Id.ToString()
                });
                ViewBag.UserList = UserList;
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
                        _db.User.Add(user);
                        _db.SaveChanges();
                        TempData["success"] = "User Added Successfully !";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["error"] = "Email already exists..!";
                        return RedirectToAction("Index");
                    }
                }
                return View();

            }
            catch (Exception ex)
            {

                return RedirectToAction("ErrorIndex", "Error", new { area = "Customer" });
            }
         
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            try
            {
                if (id == null || id == 0)
                {
                    return NotFound();
                }
                IEnumerable<SelectListItem> UserList = _db.UserType.Select(c => new SelectListItem()
                {
                    Text = c.Users,
                    Value = c.Id.ToString()
                });
                ViewBag.UserList = UserList;
                var user = _db.User.Find(id);

                if (user == null)
                {
                    return NotFound();
                }
                return View(user);

            }
            catch (Exception ex)
            {

                return RedirectToAction("ErrorIndex", "Error", new { area = "Customer" });
            }
          
        }

        [HttpPost]
        public IActionResult Edit(User user)
        {
            try
            {
                //var product = _db.Products.Find(Mdlproduct.Id);
                if (user != null)
                {
                    /* var user1 = (from e in _db.User
                                  where e.Id == user.Id
                                  select e).FirstOrDefault();*/
                    //  if (user1!=user) {
                    //  Console.WriteLine("dbhiq"+Mdlproduct);
                    var user1 = _db.User.FirstOrDefault(u => u.UserName == user.UserName &&
                     u.Email == user.Email && u.UserType == user.UserType && u.StreetAddress == user.StreetAddress
                     && u.City == user.City && u.State == user.State && u.PostalCode == user.PostalCode
                     && u.PhoneNumber == user.PhoneNumber);
                    if (user1 == null)
                    {
                        user.CreatedDate = user.CreatedDate;
                        user.ModifiedDate = DateTime.Now;
                        _db.User.Update(user);
                        _db.SaveChanges();
                        TempData["success"] = "User Updated Successfully !";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["warning"] = "Nothing get update..!";
                        return RedirectToAction("Index");
                    }
                }

                return View();

            }
            catch (Exception ex)
            {

                return RedirectToAction("ErrorIndex", "Error", new { area = "Customer" });
            }
        
        }
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            try
            {
                if (id == null || id == 0)
                {
                    return NotFound();
                }
                var user = _db.User.Find(id);

                if (user == null)
                {
                    return NotFound();
                }
                return View(user);

            }
            catch (Exception ex)
            {

                return RedirectToAction("ErrorIndex", "Error", new { area = "Customer" });
            }
          
        }

        [HttpPost]

        public IActionResult Delete(User user)
        {
            try
            {
                if (user != null)
                {
                    // var deleterecord = _db.Products.Find(id);
                    _db.User.Remove(user);
                    // _db.Products.Remove(deleterecord);
                    _db.SaveChanges();
                    TempData["success"] = "User Deleted Successfully !";
                    return RedirectToAction("Index");
                }
                return View();

            }
            catch (Exception ex)
            {

                return RedirectToAction("ErrorIndex", "Error", new { area = "Customer" });
            }
            //Product product = _db.Products.Find(Mdlproduct.ProductID);
           
        }


    }
}
