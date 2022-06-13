namespace CoreCodeFirst.Repository.IRepository
{
    public interface IUnitOfWorkRepository
    {
        IProductRepository ProductRep { get; }
        ICategoryRepository CategoryRep { get; }
        IShoppingCartRepository ShoppingCartRep { get; }
        IOrderHeaderRepository OrderHeaderRep { get; }
        IOrderDetailsRepository OrderDetailsRep { get; }
        void Save();
    }
}
