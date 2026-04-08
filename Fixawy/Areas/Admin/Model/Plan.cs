namespace Fixawy.Areas.Admin.Model
{
    public class Plan
    {
        public long Id { get; set; }
        [Required]
        [MaxLength(1000)]
        public string Name { get; set; } = string.Empty;       // Basic - Premium - Pro
        public decimal Price { get; set; }        // 100 - 200 - ...
        public int DurationInDays { get; set; }   // 30 - 90 - 365
        public bool IsActive { get; set; } = true;
        //public ICollection<Subscription> Subscriptions { get; set; }

    }
}
