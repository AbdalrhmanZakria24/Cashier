namespace Fixawy.Areas.Admin.DTOS.Request.Branch
{
    public class UpdateBranch
    {
        [MaxLength(1000)]
        public string? Name { get; set; } 
        [MaxLength(1000)]
        public string? Address { get; set; } 
        public bool? IsActive { get; set; }
        [Required]
        public long TenantId { get; set; }
    }
}
