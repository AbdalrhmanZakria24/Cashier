namespace Cashier.Domain.Entities
{
    public class OrderItem
    {
        public long Id { get; set; }
        public long OrderId { get; set; }
        public Order Order { get; set; } = null!;

        public string? PhoneNumber { get; set; }

        public long ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public decimal Quantity { get; set; }

        public decimal Discount { get; set; } = 0;
        public decimal Tax { get; set; } = 0;

        public decimal UnitPrice { get; set; }
        public decimal Total { get; set; }

    }
}
