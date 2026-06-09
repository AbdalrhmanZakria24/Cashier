namespace Fixawy.Repositories.Interface
{
    public interface ICartItemReposatory : IReposatory<CartItem>
    {
        void RemoveRange(IEnumerable<CartItem> items);
    }
}
