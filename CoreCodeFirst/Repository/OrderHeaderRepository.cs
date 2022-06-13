using CoreCodeFirst.Data;
using CoreCodeFirst.Models;
using CoreCodeFirst.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CoreCodeFirst.Repository
{
 /*   public class CategoryRepository
    {
    }*/
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly DatabaseContext _db;
        internal DbSet<OrderHeader> dbset;
        public OrderHeaderRepository(DatabaseContext db) : base(db)
        {
            _db = db;
            this.dbset = _db.Set<OrderHeader>();
        }
        public List<OrderHeader> GetAll(Expression<Func<OrderHeader, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<OrderHeader> query = dbset;
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
        /*     public void Save()
             {
                 _db.SaveChanges();
             }*/

        public void Update(OrderHeader OrderHeader)
        {
            _db.Update(OrderHeader);
        }
        public void UpdateStatus(int id,string orderStatus,string? paymentStatus = null)
        {
            var orderFromDb=_db.OrderHeader.FirstOrDefault(x => x.Id == id);
            if (orderFromDb != null)
            {
                orderFromDb.OrderStatus = orderStatus;
                if(paymentStatus != null)
                {
                    orderFromDb.PaymentStatus = paymentStatus;
                }
            }
        }
        public void UpdateStripePaymentID(int id,string sessionId,string paymentItemId)
        {
            var orderFromDb=_db.OrderHeader.FirstOrDefault(u=>u.Id == id);
            orderFromDb.PaymentDate = DateTime.Now;
            orderFromDb.SessionId = sessionId;
            orderFromDb.PaymentIntentId = paymentItemId;
        }
       

    }
}
