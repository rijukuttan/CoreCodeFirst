using CoreCodeFirst.Models;
using System.Linq.Expressions;

namespace CoreCodeFirst.Repository.IRepository
{
  
    public interface IOrderHeaderRepository : IRepository<OrderHeader>
    { 
        List <OrderHeader> GetAll(Expression<Func<OrderHeader, bool>>? filter = null, string? includeProperties = null);
        void Update(OrderHeader OrderHeader);
        // void Save();
        void UpdateStatus(int id, string orderStatus, string? paymentStatus = null);
        void UpdateStripePaymentID(int id, string sessionId, string paymentItemId);
    }
}
