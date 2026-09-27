namespace Cashier.Domain.Entities
{
    public class Cart
    {
        public long Id { get; set; }

        public long TenantId { get; set; }
        public Tenant Tenant { get; set; } = null!;

        public string ApplicationUserId { get; set; } = string.Empty;
        public ApplicationUser ApplicationUser { get; set; } = null!;

        public bool IsActive { get; set; } = true;

        public decimal TotalAmount { get; set; }

        public int ItemsCount { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
