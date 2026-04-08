namespace Fixawy.Areas.Admin.Model
{
    public class DeviceSync
    {
        public long Id { get; set; }

        public long TenantId { get; set; }
        public Tenant Tenant { get; set; } = null!;

        public long BranchId { get; set; }
        public Branch Branch { get; set; } =null!;
        [Required]
        [MaxLength(1000)]
        public string Name { get; set; } = string.Empty;
        
        public DateTime LastSyncAt { get; set; }
    }
}
