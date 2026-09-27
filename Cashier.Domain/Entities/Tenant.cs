namespace Cashier.Domain.Entities
{
    public class Tenant
    {
        public long Id { get; set; }


        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public bool HaveBranches { get; set; }

        public DateTime SubscriptionEndDate { get; set; }
        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}