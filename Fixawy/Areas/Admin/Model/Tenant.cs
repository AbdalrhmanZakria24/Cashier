namespace Fixawy.Areas.Admin.Model
{
    public class Tenant
    {
        public long Id { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public bool HaveBranches { get; set; }

        public DateTime SubscriptionEndDate { get; set; }
        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}