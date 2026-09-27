namespace Cashier.Application.Interface
{
    public interface ICartItemReposatory : IRepository<CartItem>
    {
        void RemoveRange(IEnumerable<CartItem> items);
    }
}
