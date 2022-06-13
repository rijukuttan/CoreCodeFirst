using CoreCodeFirst.Data;
using CoreCodeFirst.Models;
using CoreCodeFirst.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CoreCodeFirst.Controllers
{
    public class ProductController : Controller
    {
        /*  public readonly DatabaseContext _db;
          public ProductController(DatabaseContext db)
          {
              _db = db;
          }*/
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostEnvironment;


        public readonly IUnitOfWorkRepository _db;
        public ProductController(IUnitOfWorkRepository db , Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment)
        {
            _hostEnvironment = _environment;
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<Product> ProductList;
            try { 
            ProductList = (IEnumerable<Product>)_db.ProductRep.GetAll();
            return View(ProductList);
            }
            catch (Exception ex)
            {

                return RedirectToAction("ErrorIndex", "Error",new { area = "Customer" });
            }
            

        }
        public IActionResult CreateProduct()
        {
            try
            {
                IEnumerable<SelectListItem> CategoryList = _db.CategoryRep.GetAll().Select(c => new SelectListItem()
                {
                    Text = c.CategoryName,
                    Value = c.Id.ToString()
                });
                ViewBag.CategoryList = CategoryList;
                // IEnumerable<Product> ProductList = _db.Products;
                return View();

            }
            catch (Exception ex)
            {

                return RedirectToAction("ErrorIndex", "Error", new { area = "Customer" });
            }
      
        }
        [HttpPost]
        public IActionResult CreateProduct(Product product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Product productModel = new Product();
                    product.CreateDate = DateTime.Now;
                    product.ModifyDate = DateTime.Now;
                    _db.ProductRep.Add(product);
                    _db.Save();
                    TempData["success"] = "product Added Successfully !";
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
                IEnumerable<SelectListItem> CategoryList = _db.CategoryRep.GetAll().Select(c => new SelectListItem()
                {
                    Text = c.CategoryName,
                    Value = c.Id.ToString()
                });
                ViewBag.CategoryList = CategoryList;

                if (id == null || id == 0)
                {
                    TempData["error"] = "id is null !";
                    return NotFound();
                }
                var product = _db.ProductRep.GetFirstOrDefault(c => c.Id == id);

                if (product == null)
                {
                    return NotFound();
                }
                else
                {
                    return View(product);
                }
               

            }
            catch (Exception ex)
            {

                return RedirectToAction("ErrorIndex", "Error", new { area = "Customer" });
            }
           
        }
        [HttpPost]       
        public IActionResult Edit(Product Mdlproduct)
        {
            try
            {
                if (Mdlproduct != null)
                {
                    //  Console.WriteLine("dbhiq"+Mdlproduct);
                    Mdlproduct.CreateDate = Mdlproduct.CreateDate;
                    Mdlproduct.ModifyDate = DateTime.Now;
                    _db.ProductRep.Update(Mdlproduct);
                    _db.Save();
                    TempData["success"] = "product Updated Successfully !";
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
                var product = _db.ProductRep.GetFirstOrDefault(c => c.Id == id);
                if (product == null)
                {
                    TempData["error"] = "product is null !";
                    return NotFound();
                }
                return View(product);

            }
            catch (Exception ex)
            {

                return RedirectToAction("ErrorIndex", "Error", new { area = "Customer" });
            }
            
        }
        [HttpPost]
        public IActionResult Delete(Product product)
        {
            try
            {
                if (product != null)
                {
                    // var deleterecord = _db.Products.Find(id);
                    _db.ProductRep.Remove(product);
                    // _db.Products.Remove(deleterecord);
                    _db.Save();
                    TempData["success"] = "product Deleted Successfully !";
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
        /*        [HttpPost]
                public IActionResult Delete()
                {
                    TempData["success"] = "delete block !";
                    //Product product = _db.Products.Find(Mdlproduct.ProductID);
                    *//*if (id != null)
                       {
                           var deleterecord = _db.Products.Find(id);
                           // _db.User.Remove(deleterecord);
                           _db.Products.Remove(deleterecord);
                           _db.SaveChanges();
                           TempData["success"] = "Data Deleted Successfully !";
                           return RedirectToAction("Index");
                       }*//*
                    return RedirectToAction("Index");
                    //return View();
                }*/

        /*        public IActionResult d()
                {
                    TempData["success"] = "delete block !";
                    //Product product = _db.Products.Find(Mdlproduct.ProductID);
                    *//*if (id != null)
                       {
                           var deleterecord = _db.Products.Find(id);
                           // _db.User.Remove(deleterecord);
                           _db.Products.Remove(deleterecord);
                           _db.SaveChanges();
                           TempData["success"] = "Data Deleted Successfully !";
                           return RedirectToAction("Index");
                       }*//*
                    return RedirectToAction("Index");
                    //return View();
                }*/
        //edit
        [HttpGet]
        public IActionResult EditOrInsert(int? id)
        {
            try
            {
                IEnumerable<SelectListItem> CategoryList = _db.CategoryRep.GetAll().Select(c => new SelectListItem()
                {
                    Text = c.CategoryName,
                    Value = c.Id.ToString()
                });
                ViewBag.CategoryList = CategoryList;
              /*  Dropdownlist multi_Dropdownlist = new Dropdownlist
                {
                    custlist = GetCustomerList(),
                    emplist = GetEmployeeList()
                };
                return View(multi_Dropdownlist);*/
                if (id == null || id == 0)
                {
                    /* Product pro = new Product();
                     pro.Id = 0;*/
                    ViewBag.mode = "create";
                    // TempData["error"] = "id is null !";
                    return View();
                }
                else
                {
                    var product = _db.ProductRep.GetFirstOrDefault(c => c.Id == id);
                    ViewBag.mode = "update";
                    if (product == null)
                    {
                        return NotFound();
                    }
                    return View(product);
                }

            }
            catch (Exception ex)
            {

                return RedirectToAction("ErrorIndex", "Error", new { area = "Customer" });
            }
           
        }
        
        [HttpPost]
        public IActionResult EditOrInsert(Product Mdlproduct,IFormFile file)
        {
            try
            {
                var wwwRootPath = _hostEnvironment.WebRootPath;
                if (file != null)
                {
                    var fileName = Guid.NewGuid().ToString();
                    var extension = Path.GetExtension(file.FileName);
                    var uploadPath = Path.Combine(wwwRootPath, @"Image\Product");
                    if (Mdlproduct.ImageUrl != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, Mdlproduct.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    using (var fileStreams = new FileStream(Path.Combine(uploadPath, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    Mdlproduct.ImageUrl = @"\Image\Product\" + fileName + extension;
                }
                //var product = _db.Products.Find(Mdlproduct.Id);
                if (Mdlproduct != null)
                {
                    if (Mdlproduct.Id == 0 || Mdlproduct.Id == null)
                    {
                        Mdlproduct.CreateDate = DateTime.Now;
                        Mdlproduct.ModifyDate = DateTime.Now;
                        _db.ProductRep.Add(Mdlproduct);
                        _db.Save();
                        TempData["success"] = "product Added Successfully !";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        Mdlproduct.CreateDate = Mdlproduct.CreateDate;
                        Mdlproduct.ModifyDate = DateTime.Now;
                        _db.ProductRep.Update(Mdlproduct);
                        _db.Save();
                        TempData["success"] = "product Updated Successfully !";
                        return RedirectToAction("Index");
                    }
                    //  Console.WriteLine("dbhiq"+Mdlproduct);            
                }
                return View();
            }
            catch (FileNotFoundException ex)
            {
                return Redirect("Error.cshtml");
            }
            catch (Exception ex1)
            {
                return Redirect("Error.cshtml");
            }

           
        }
        #region Implementation for api calls
        [HttpDelete]
        public IActionResult DeleteProduct(int? id)
        {
            try
            {
                if (id == null)
                {
                    TempData["error"] = " id is null!";
                    return Json(new { status = "Failed", message = "deletion of product failed" });
                }
                else
                {
                    var product = _db.ProductRep.GetFirstOrDefault(c => c.Id == id);
                    if (product == null)
                    {
                        TempData["error"] = "product Deletion failed !";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        _db.ProductRep.Remove(product);
                        // _db.Products.Remove(deleterecord);
                        _db.Save();
                        TempData["success"] = "product Deleted Successfully !";
                        // return Json(new { status = "success" });
                        return RedirectToAction("Index");
                    }
                }

            }
            catch (Exception ex)
            {

                return RedirectToAction("ErrorIndex", "Error", new { area = "Customer" });
            }
           
        }
        #endregion
    }
}
