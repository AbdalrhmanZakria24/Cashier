namespace Fixawy.Areas.Admin.DTOS.Request.Bransh
{
    public class AddBranch
    {
        [Required(ErrorMessage ="Enter branch name")]
        [MaxLength(1000)]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage ="Enter branch address")]
        [MaxLength(1000)]
        public string Address { get; set; } = string.Empty;
        [Required]
        public bool IsActive { get; set; } = true;
        [Required]
        public long TenantId { get; set; }
    }
}
