using Fixawy.Areas.Admin.Enums;
using Fixawy.Enums;

namespace Fixawy.Model
{
  
    public class Subscription
    {
        public string Id { get; set; }

        public string UserId { get; set; }
        public string TenantId { get; set; }

        public string PlanId { get; set; }
        public Plan Plan { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime ExpirationDate { get; set; }

        public SubscriptionType Type { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public SubscriptionStatus Status { get; set; }
        public decimal Price { get; set; }
        public DateTime? EndedAt { get; set; }


    }
}
