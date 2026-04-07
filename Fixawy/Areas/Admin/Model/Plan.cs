namespace Fixawy.Model
{
    public class Plan
    {
        public string Id { get; set; }
        public string Name { get; set; }          // Basic - Premium - Pro
        public decimal Price { get; set; }        // 100 - 200 - ...
        public int DurationInDays { get; set; }   // 30 - 90 - 365
        public bool IsActive { get; set; } = true;
        //public ICollection<Subscription> Subscriptions { get; set; }

    }
}
