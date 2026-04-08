    using Fixawy.Areas.Admin.Enums;
    namespace Fixawy.Areas.Admin.Model
    {
  
        public class Subscription
        {
            public long Id { get; set; } 

            public string UserId { get; set; } = string.Empty;
            public ApplicationUser ApplicationUser { get; set; } = null!;

            public long TenantId { get; set; } 
            public Tenant Tenants { get; set; } = null!;

        public long PlanId { get; set; }
        public Plan Plan { get; set; } = null!;

            public DateTime StartDate { get; set; }
            public DateTime ExpirationDate { get; set; }

            public SubscriptionType Type { get; set; }
            public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

            public SubscriptionStatus Status { get; set; }
            public decimal Price { get; set; }
            public DateTime? EndedAt { get; set; }


        }
    }
