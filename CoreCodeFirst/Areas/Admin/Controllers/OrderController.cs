using CoreCodeFirst.Models;
using CoreCodeFirst.Repository.IRepository;
using CoreCodeFirst.Utilities;
using CoreCodeFirst.ViewModels;
//using System.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;


namespace CoreCodeFirst.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        public readonly IUnitOfWorkRepository _unitOfWork;
        [BindProperty]
        public OrderVM OrderVM { get; set; }
        public OrderController(IUnitOfWorkRepository db)
        {
            _unitOfWork = db;
        }
        public IActionResult Index(string? status)
        {
            if (status == null)
            {
                IEnumerable<OrderHeader> orderList = _unitOfWork.OrderHeaderRep.GetAll();
                return View(orderList);
            }
            else if (status == "all") {
                List<OrderHeader> orderList = new List<OrderHeader>();
                orderList = _unitOfWork.OrderHeaderRep.GetAll();
                //return PartialView("_OrderTable", orderList);
                return Json(orderList);
            }
            else
            {
                List<OrderHeader> orderList = new List<OrderHeader>();
                orderList = _unitOfWork.OrderHeaderRep.GetAll(u => u.OrderStatus == status);
                return Json(orderList);
                //return View(db.tbProducts.ToList());
                // IEnumerable<OrderHeader> orderList = _unitOfWork.OrderHeaderRep.GetAll(u=>u.OrderStatus==status);
                // return Json(orderList);
                // return Json(orderList);
            }           
        }
        public IActionResult Details(int orderId)
        {
            OrderVM = new OrderVM()
            {
                OrderHeader = _unitOfWork.OrderHeaderRep.GetFirstOrDefault(U => U.Id == orderId),
                OrderDetails = _unitOfWork.OrderDetailsRep.GetAll(u => u.OrderId == orderId, includeProperties: "Product")
            };
            return View(OrderVM);
        } 
        [ActionName("Details")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details_PAY_NOW()
        {
            OrderVM.OrderHeader = _unitOfWork.OrderHeaderRep.GetFirstOrDefault(u => u.Id == OrderVM.OrderHeader.Id);
            OrderVM.OrderDetails = _unitOfWork.OrderDetailsRep.GetAll(u => u.OrderId == OrderVM.OrderHeader.Id, includeProperties: "Product");
            var domain = "https://localhost:44300/";
            var option = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> {
                "card",
                },
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = domain + $"admin/order/PaymentConfirmation?orderHeaderid={OrderVM.OrderHeader.Id}",
                CancelUrl = domain + $"admin/order/details?orderId={OrderVM.OrderHeader.Id}",
            };
            foreach (var item in OrderVM.OrderDetails)
            {
                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount=(long)(item.Price*100),
                        Currency="usd",
                        ProductData=new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.ProductName
                        },
                       
                    },
                    Quantity = item.Count,
                };
                option.LineItems.Add(sessionLineItem);
            }
            var service = new SessionService();
            Session session = service.Create(option);
            _unitOfWork.OrderHeaderRep.UpdateStripePaymentID(OrderVM.OrderHeader.Id, session.Id, session.PaymentIntentId);
            _unitOfWork.Save();
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }
        public IActionResult PaymentConfirmation(int orderHeaderid)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeaderRep.GetFirstOrDefault(u => u.Id == orderHeaderid);
            if (orderHeader.PaymentStatus == AppConstants.PaymentStatusDelayedPayment)
            {
                var service = new SessionService();
                Session session = service.Get(orderHeader.SessionId);
                if (session.PaymentStatus.ToLower() == "paid")
                {
                    _unitOfWork.OrderHeaderRep.UpdateStatus(orderHeaderid, AppConstants.StatusApproved, AppConstants.PaymentStatusApproved);
                    _unitOfWork.Save();
                }
            }
            return View(orderHeaderid);
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateOrderDetail()
        {
            var orderHeaderFromDb = _unitOfWork.OrderHeaderRep.GetFirstOrDefault
                (u => u.Id == OrderVM.OrderHeader.Id);
            orderHeaderFromDb.Name = OrderVM.OrderHeader.Name;
            orderHeaderFromDb.PhoneNumber = OrderVM.OrderHeader.PhoneNumber;
            orderHeaderFromDb.StreetAddress = OrderVM.OrderHeader.StreetAddress;
            orderHeaderFromDb.City = OrderVM.OrderHeader.City;
            orderHeaderFromDb.State = OrderVM.OrderHeader.State;
            orderHeaderFromDb.PostalCode = OrderVM.OrderHeader.PostalCode;
            if (OrderVM.OrderHeader.Carrier != null)
            {
                orderHeaderFromDb.Carrier = OrderVM.OrderHeader.Carrier;
            }
            if (OrderVM.OrderHeader.TrackingNumber != null)
            {
                orderHeaderFromDb.TrackingNumber = OrderVM.OrderHeader.TrackingNumber;
            }
            _unitOfWork.OrderHeaderRep.Update(orderHeaderFromDb);
            _unitOfWork.Save();
            TempData["success"] = "order status Updated Successfully.";
            return RedirectToAction("Details", "Order", new { orderId = OrderVM.OrderHeader.Id });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult StartProcessing()
        {

            _unitOfWork.OrderHeaderRep.UpdateStatus(OrderVM.OrderHeader.Id, AppConstants.StatusInProcess);
            _unitOfWork.Save();
            TempData["success"] = "order status Start Successfully.";
            return RedirectToAction("Details", "Order", new { orderId = OrderVM.OrderHeader.Id });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ShipOrder()
        {
            var orderHeader = _unitOfWork.OrderHeaderRep.GetFirstOrDefault
                (u => u.Id == OrderVM.OrderHeader.Id);
            orderHeader.TrackingNumber = OrderVM.OrderHeader.TrackingNumber;
            orderHeader.Carrier = OrderVM.OrderHeader.Carrier;
            orderHeader.OrderStatus = AppConstants.StatusShipped;
            orderHeader.ShippingDate = DateTime.Now;
            if (orderHeader.PaymentStatus == AppConstants.PaymentStatusDelayedPayment)
            {
                orderHeader.PaymentDueDate = DateTime.Now.AddDays(30);
            }
            _unitOfWork.OrderHeaderRep.Update(orderHeader);
            _unitOfWork.Save();
            return RedirectToAction("Details", "Order", new { orderId = OrderVM.OrderHeader.Id });


        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CancelOrder()
        {

            var orderHeader = _unitOfWork.OrderHeaderRep.GetFirstOrDefault(u => u.Id == OrderVM.OrderHeader.Id);
            if (orderHeader.PaymentStatus == AppConstants.PaymentStatusApproved)
            {
                var options = new Stripe.RefundCreateOptions
                {
                    Reason = Stripe.RefundReasons.RequestedByCustomer,
                    PaymentIntent = orderHeader.PaymentIntentId
                };
                var service = new RefundService();
                Refund refund = service.Create(options);
                _unitOfWork.OrderHeaderRep.UpdateStatus(orderHeader.Id, AppConstants.StatusCancelled, AppConstants.StatusRefunded);

            }
            else
            {
                _unitOfWork.OrderHeaderRep.UpdateStatus(orderHeader.Id, AppConstants.StatusCancelled, AppConstants.StatusCancelled);
            }
            _unitOfWork.Save();
            TempData["success"] = "order Cancelled Successfully.";
            return RedirectToAction("Details", "Order", new { orderId = OrderVM.OrderHeader.Id });
        }

    }

}

