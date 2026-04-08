namespace Fixawy.Areas.Admin.Model
{
    public class OrderItem
    {
        public long Id { get; set; }

        public long TenantId { get; set; }

        public long OrderId { get; set; }
        public Order Order { get; set; } = null!;

        public long ProductId { get; set; }

        public decimal Quantity { get; set; }

        public decimal Price { get; set; }
        public decimal Total { get; set; }

    }
}
