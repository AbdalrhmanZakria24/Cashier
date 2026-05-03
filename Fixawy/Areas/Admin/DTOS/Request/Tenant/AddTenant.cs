namespace Fixawy.Areas.Admin.DTOS.Request.Tenant
{
    public class AddTenant
    {
        [Required]
        [MaxLength(1000)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public bool HaveBranches { get; set; }
    }
}
