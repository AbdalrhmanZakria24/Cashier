namespace Fixawy.Repositories
{
    public class CartItemReposatory : Reposatory<CartItem>, ICartItemReposatory
    {
        private readonly ApplicationDBContext _context;

        public CartItemReposatory(ApplicationDBContext context)
            : base(context)
        {
            _context = context;
        }

        public void RemoveRange(IEnumerable<CartItem> items)
        {
            _context.CartItems.RemoveRange(items);
        }
    }
}
