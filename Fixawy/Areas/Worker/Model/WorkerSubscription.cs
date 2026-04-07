namespace Fixawy.Areas.Worker.Model
{
    public class WorkerSubscription
    {
        public int Id { get; set; }

        public string UserId { get; set; } = string.Empty;
        public ApplicationUser ApplicationUser { get; set; }

        public int SubscriptionPlanId { get; set; }
        //public SubscriptionPlan Plan { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int MaxUse {  get; set; }

        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
