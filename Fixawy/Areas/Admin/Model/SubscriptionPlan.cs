namespace Fixawy.Areas.Admin.Model
{
    public enum SubscriptionPlanStatus
    {
        Basic,
        Premium
    }
    public class SubscriptionPlan
    {
        public int Id { get; set; }

        public SubscriptionPlanStatus PlanStatus { get; set; }
        public double Price { get; set; }
        public int DurationInDays {  get; set; }
        public string? Description { get; set; } = string.Empty;

    }
}
