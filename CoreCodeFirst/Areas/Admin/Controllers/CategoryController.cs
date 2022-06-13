using CoreCodeFirst.Models;
using CoreCodeFirst.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace CoreCodeFirst.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        public readonly IUnitOfWorkRepository _db;
        public CategoryController(IUnitOfWorkRepository db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            try
            {
                IEnumerable<Category> CategoryList = (IEnumerable<Category>)_db.CategoryRep.GetAll();
                return View(CategoryList);

            }
            catch (Exception ex)
            {

                return RedirectToAction("ErrorIndex", "Error", new { area = "Customer" });
            }
          
        }
        public IActionResult CreateCategory()
        {
            // IEnumerable<Product> ProductList = _db.Products;
            return View();
        }
        [HttpPost]
        public IActionResult CreateCategory(Category category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Product productModel = new Product();

                    _db.CategoryRep.Add(category);
                    _db.Save();
                    TempData["success"] = "category Added Successfully !";
                    return RedirectToAction("Index");
                }
                return View();

            }
            catch (Exception ex)
            {

                return RedirectToAction("ErrorIndex", "Error", new { area = "Customer" });
            }
         
        }

        //edit
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            try
            {
                if (id == null || id == 0)
                {
                    TempData["error"] = "id is null !";
                    return NotFound();
                }
                var catogory = _db.CategoryRep.GetFirstOrDefault(c => c.Id == id);

                if (catogory == null)
                {
                    return NotFound();
                }
                return View(catogory);

            }
            catch (Exception ex)
            {

                return RedirectToAction("ErrorIndex", "Error", new { area = "Customer" });
            }
        
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {
            try
            {
                if (category != null)
                {
                    //  Console.WriteLine("dbhiq"+Mdlproduct);

                    _db.CategoryRep.Update(category);
                    _db.Save();
                    TempData["success"] = "category Updated Successfully !";
                    return RedirectToAction("Index");

                }

                return View();

            }
            catch (Exception ex)
            {

                return RedirectToAction("ErrorIndex", "Error", new { area = "Customer" });
            }
            //var product = _db.Products.Find(Mdlproduct.Id);
           
        }
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            try
            {
                if (id == null || id == 0)
                {
                    TempData["error"] = "id is null !";
                    return NotFound();
                }
                var category = _db.CategoryRep.GetFirstOrDefault(c => c.Id == id);

                if (category == null)
                {
                    TempData["error"] = "category is null !";
                    return NotFound();
                }
                return View(category);

            }
            catch (Exception ex)
            {

                return RedirectToAction("ErrorIndex", "Error", new { area = "Customer" });
            }

        }

        [HttpPost]

        public IActionResult Delete(Category category)
        {
            try
            {

                if (category != null)
                {
                    // var deleterecord = _db.Products.Find(id);
                    _db.CategoryRep.Remove(category);
                    // _db.Products.Remove(deleterecord);
                    _db.Save();
                    TempData["success"] = "category Deleted Successfully !";

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
