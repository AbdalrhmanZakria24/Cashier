
namespace Fixawy.Areas.Admin.Model
{
    public class Branch
    {
        public long Id { get; set; }

        public long TenantId { get; set; }
        public Tenant Tenant { get; set; } = null!;
        [Required]
        [MaxLength(1000)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [MaxLength(1000)]
        public string Address { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
    }
}
