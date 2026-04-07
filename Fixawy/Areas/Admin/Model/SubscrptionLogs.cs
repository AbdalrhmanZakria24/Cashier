using Fixawy.Areas.Admin.Enums;
using Fixawy.Model;

namespace Fixawy.Areas.Admin.Model
{
    public class SubscriptionLogs
    {
        public string Id { get; set; }

        public string TenantId { get; set; }
        public string UserId { get; set; }

        public string SubscriptionId { get; set; }

        public SubscriptionAction Action { get; set; }
        public Subscription Subscription { get; set; }

        // Snapshot
        public string PlanId { get; set; }
        public string PlanName { get; set; }
        public decimal Price { get; set; }
        public int Duration { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime ExpirationDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; }
        public DateTime? EffectiveEndDate { get; set; }
    }
}
