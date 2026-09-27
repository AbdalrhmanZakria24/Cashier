namespace Cashier.Domain.Entities
{
    public class Order
    {
        public long Id { get; set; }

        public long TenantId { get; set; }
        public Tenant Tenant { get; set; } = null!;
        public Invoice? Invoice { get; set; }
        public string UserId { get; set; } = string.Empty;

        public Payment? Payment { get; set; }

        public decimal SubTotal { get; set; }
        public decimal Discount { get; set; } = 0;
        public decimal Tax { get; set; } = 0;
        public decimal Total { get; set; }
        public string? Notes { get; set; }

        public OrderStatus? Status { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool Synced { get; set; } = false;

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();


    }
}
