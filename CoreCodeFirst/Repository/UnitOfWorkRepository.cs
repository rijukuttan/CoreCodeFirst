using CoreCodeFirst.Data;

using CoreCodeFirst.Repository.IRepository;

namespace CoreCodeFirst.Repository
{
    public class UnitOfWorkRepository:IUnitOfWorkRepository
    {
        private readonly DatabaseContext _db;
        public IProductRepository ProductRep { get; private set; }
        public ICategoryRepository CategoryRep { get; private set; }
        public IShoppingCartRepository ShoppingCartRep { get; private set; }
        public IOrderDetailsRepository OrderDetailsRep { get;}
        public IOrderHeaderRepository OrderHeaderRep { get;}
        // public IProductRepository ProductRepository => throw new NotImplementedException();
        public UnitOfWorkRepository(DatabaseContext db)
        {
            _db = db;
            ProductRep = new ProductRepository(_db);
            CategoryRep = new CategoryRepository(_db);
            ShoppingCartRep = new ShoppingCartRepository(_db);
            OrderDetailsRep = new OrderDetailsRepository(_db);
            OrderHeaderRep = new OrderHeaderRepository(_db);
        }
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
