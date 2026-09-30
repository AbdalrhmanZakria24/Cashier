namespace Cashier.Domain.Entities
{
    public enum PaymentStatus
    {
        Pending,
        Paid,
        Failed,
        Refunded
    }
    public enum PaymentMethod
    {
        Cash,
        Card,
        Wallet
    }
    public class Payment
    {
        public long Id { get; set; }

        public long cartId { get; set; }
        public Cart cart { get; set; } = null!;

        public string ApplicationUserId { get; set; } = string.Empty;
        public ApplicationUser ApplicationUser { get; set; } = null!;

        public decimal Amount { get; set; }

        public PaymentStatus Status { get; set; }

        public PaymentMethod Method { get; set; }

        public string? TransactionId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? PaidAt { get; set; }

        public ICollection<PaymentTransaction> Transactions { get; set; } = new List<PaymentTransaction>();

    }
}
