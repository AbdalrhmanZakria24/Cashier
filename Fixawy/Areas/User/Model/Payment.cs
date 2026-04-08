namespace Fixawy.Areas.User.Model
{
    public enum PaymentMethod
    {
        wallet,
        visa,
    }
    public class Payment
    {
        public long Id { get; set; }

        public long TenantId { get; set; }
        public Tenant Tenant { get; set; } = null!;

        public string UserId { get; set; } = string.Empty;
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;

        public double Amount { get; set; }
        public DateTime PaymentDate { get; set; }

        public PaymentMethod PaymentMethod { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
