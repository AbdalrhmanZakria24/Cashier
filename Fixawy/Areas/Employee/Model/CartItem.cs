namespace Fixawy.Areas.Employee.Model
{
    public class CartItem
    {
        public long Id { get; set; }

        public long CartId { get; set; }
        public Cart Cart { get; set; } = null!;

        public long ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public decimal Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal Total {  get; set; }

        public decimal Discount { get; set; } = 0;

        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    }
}
