using Fixawy.Areas.Admin.Enums;

namespace Fixawy.Areas.Admin.Model
{
    public class SubscriptionLogs
    {
        public long Id { get; set; }

        public long TenantId { get; set; } 
        public Tenant Tenants { get; set; } = null!;


        public string UserId { get; set; } = string.Empty;
        public ApplicationUser ApplicationUser { get; set; } = null!; 

        public long SubscriptionId { get; set; } 
        public SubscriptionAction Action { get; set; }
        public Subscription Subscription { get; set; } = null!;

        // Snapshot
        public long PlanId { get; set; } 
        public Plan Plan { get; set; } = null!;
        public string PlanName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Duration { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime ExpirationDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; }
        public DateTime? EffectiveEndDate { get; set; }
    }
}
