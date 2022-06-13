using Microsoft.AspNetCore.Mvc;

namespace CoreCodeFirst.Areas.Customer.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult ErrorIndex()
        {
            return View();
        }
    }
}
