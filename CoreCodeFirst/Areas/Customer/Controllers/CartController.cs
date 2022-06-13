using CoreCodeFirst.Data;
using CoreCodeFirst.Models;
using CoreCodeFirst.Repository.IRepository;
using CoreCodeFirst.Utilities;
using CoreCodeFirst.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
//using Stripe.BillingPortal;

namespace CoreCodeFirst.Areas.Customer.Controllers
{
    public class CartController : Controller
    {
        public readonly DatabaseContext _db;

        public readonly IUnitOfWorkRepository _unitOfWork;
        //public ShoppingCartVM ShoppingCartVM { get; set; }
        public CartController( IUnitOfWorkRepository db, DatabaseContext dbContext)
        {
           
            _unitOfWork = db;
            _db = dbContext;
   
        }
        public IActionResult Index()
        {
            try
            {
                int UserId = (int)HttpContext.Session.GetInt32("UserId");
                var ShoppingCartVM = new ShoppingCartVM()
                {

                    ListCart = _unitOfWork.ShoppingCartRep.GetCartByUser(u => u.UserId == UserId, includeProperties: "Product"),
                    // ListCart = _unitOfWork.ShoppingCartRep.GetAll(includeProperties:"Product"),
                    OrderHeader = new()
                };
                foreach (var cart in ShoppingCartVM.ListCart)
                {
                    cart.Price = (double)cart.Product.ProductPrice;
                    //cart.Price = 30;
                    ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
                }
                return View(ShoppingCartVM);

            }
            catch (Exception ex)
            {

                return RedirectToAction("ErrorIndex", "Error", new { area = "Customer" });
            }
           
        }
        public IActionResult Summary()
        {
            try
            {
                int UserId = (int)HttpContext.Session.GetInt32("UserId");
                var shoppingCartVM = new ShoppingCartVM()
                {
                    ListCart = _unitOfWork.ShoppingCartRep.GetCartByUser(u => u.UserId == UserId, includeProperties: "Product"),
                    OrderHeader = new()
                };
                var user = _db.User.FirstOrDefault(u => u.Id == UserId);
                if (user != null)
                {
                    shoppingCartVM.OrderHeader.Name = user.UserName;
                    shoppingCartVM.OrderHeader.PhoneNumber = user.PhoneNumber;
                    shoppingCartVM.OrderHeader.StreetAddress = user.StreetAddress;
                    shoppingCartVM.OrderHeader.City = user.City;
                    shoppingCartVM.OrderHeader.State = user.State;
                    shoppingCartVM.OrderHeader.PostalCode = user.PostalCode;
                    foreach (var cart in shoppingCartVM.ListCart)
                    {
                        //cart.Price=
                        //cart.Price = GetPriceBasedOnQuantity();
                        cart.Price = (double)cart.Product.ProductPrice;
                        shoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
                    }
                }

                return View(shoppingCartVM);

            }
            catch (Exception ex)
            {

                return RedirectToAction("ErrorIndex", "Error", new { area = "Customer" });
            }
           
        }
        public IActionResult Plus(int cartId)
        {
            try
            {
                int UserId = (int)HttpContext.Session.GetInt32("UserId");
                var cart = _unitOfWork.ShoppingCartRep.GetFirstOrDefault(u => u.Id == cartId & u.UserId == UserId);
                _unitOfWork.ShoppingCartRep.IncrementCount(cart, 1);
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {

                return RedirectToAction("ErrorIndex", "Error", new { area = "Customer" });
            }
          
        }
        public IActionResult Minus(int cartId)
        {
            try
            {
                int UserId = (int)HttpContext.Session.GetInt32("UserId");
                var cart = _unitOfWork.ShoppingCartRep.GetFirstOrDefault(u => u.Id == cartId & u.UserId == UserId);
                if (cart != null)
                {
                    if (cart.Count <= 1)
                    {
                        _unitOfWork.ShoppingCartRep.Remove(cart);
                        var count = _unitOfWork.ShoppingCartRep.GetAll().ToList().Count - 1;
                    }
                    else
                    {
                        _unitOfWork.ShoppingCartRep.DecrementCount(cart, 1);
                    }
                    _unitOfWork.Save();
                }

                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {

                return RedirectToAction("ErrorIndex", "Error", new { area = "Customer" });
            }
       
        }
        public int GetCount(int cartId)
        {
            try
            {
                int UserId = (int)HttpContext.Session.GetInt32("UserId");
                var cart = _unitOfWork.ShoppingCartRep.GetFirstOrDefault(u => u.Id == cartId & u.UserId == UserId);
                if (cart == null)
                {
                    return 0;
                }
                return cart.Count;

            }
            catch (Exception ex)
            {

                return RedirectToAction("ErrorIndex", "Error", new { area = "Customer" });
            }
         
        }
        public IActionResult Remove(int cartId)
        {
            try
            {
                int UserId = (int)HttpContext.Session.GetInt32("UserId");
                var cart = _unitOfWork.ShoppingCartRep.GetFirstOrDefault(u => u.Id == cartId & u.UserId == UserId);
                _unitOfWork.ShoppingCartRep.Remove(cart);
                _unitOfWork.Save();
                var count = _unitOfWork.ShoppingCartRep.GetAll().ToList().Count;
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {

                return RedirectToAction("ErrorIndex", "Error", new { area = "Customer" });
            }
          
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]
       public IActionResult SummaryPost(ShoppingCartVM shoppingCartVM)
        {
            try
            {
                int UserId = (int)HttpContext.Session.GetInt32("UserId");
                shoppingCartVM.ListCart = _unitOfWork.ShoppingCartRep.GetCartByUser(u => u.UserId == UserId, includeProperties: "Product");
                shoppingCartVM.OrderHeader.OrderDate = System.DateTime.Now;
                foreach (var cart in shoppingCartVM.ListCart)
                {
                    shoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
                }
                shoppingCartVM.OrderHeader.PaymentStatus = AppConstants.PaymentStatusPending;
                shoppingCartVM.OrderHeader.OrderStatus = AppConstants.StatusPending;

                _unitOfWork.OrderHeaderRep.Add(shoppingCartVM.OrderHeader);
                _unitOfWork.Save();
                foreach (var cart in shoppingCartVM.ListCart)
                {
                    cart.Price = (double)cart.Product.ProductPrice;
                    OrderDetails orderDetail = new()
                    {
                        ProductId = cart.ProductId,
                        OrderId = shoppingCartVM.OrderHeader.Id,
                        Price = cart.Price,
                        Count = cart.Count
                    };
                    _unitOfWork.OrderDetailsRep.Add(orderDetail);
                    _unitOfWork.Save();
                }
                var domainUrl = "https://localhost:44336/";
                var options = new SessionCreateOptions
                {
                    LineItems = new List<SessionLineItemOptions>(),
                    Mode = "payment",
                    SuccessUrl = domainUrl + $"Customer/cart/OrderConfirmation?id={shoppingCartVM.OrderHeader.Id}",
                    CancelUrl = domainUrl + $"customer/cart/index",
                };
                foreach (var item in shoppingCartVM.ListCart)
                {
                    var sessionLineItem = new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(item.Price * 100),
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.Product.ProductName
                            },
                        },
                        Quantity = item.Count,
                    };
                    options.LineItems.Add(sessionLineItem);
                }
                var service = new SessionService();
                Session session = service.Create(options);
                _unitOfWork.OrderHeaderRep.UpdateStripePaymentID(shoppingCartVM.OrderHeader.Id, session.Id, session.PaymentIntentId);
                _unitOfWork.Save();
                Response.Headers.Add("Location", session.Url);
                return new StatusCodeResult(303);

            }
            catch (Exception ex)
            {

                return RedirectToAction("ErrorIndex", "Error", new { area = "Customer" });
            }
         
        }
        public IActionResult OrderConfirmation(int id)
        {
            try
            {
                int UserId = (int)HttpContext.Session.GetInt32("UserId");
                var cart = _unitOfWork.ShoppingCartRep.GetCartByUser(u => u.UserId == UserId);
                OrderHeader orderHeader = _unitOfWork.OrderHeaderRep.GetFirstOrDefault(u => u.Id == id);
                if (orderHeader.PaymentStatus != AppConstants.PaymentStatusDelayedPayment)
                {
                    var service = new SessionService();
                    Session session = service.Get(orderHeader.SessionId);
                    if (session.PaymentStatus.ToLower() == "paid")
                    {
                        _unitOfWork.OrderHeaderRep.UpdateStatus(id, AppConstants.StatusApproved, AppConstants.PaymentStatusApproved);
                        _unitOfWork.ShoppingCartRep.RemoveRange(cart);
                    }
                }
                List<ShoppingCart> shoppingCart = _unitOfWork.ShoppingCartRep.GetAll().ToList();

                _unitOfWork.Save();
                return View(id);

            }
            catch (Exception ex)
            {

                return RedirectToAction("ErrorIndex", "Error", new { area = "Customer" });
            }
       
        }
    }
}
