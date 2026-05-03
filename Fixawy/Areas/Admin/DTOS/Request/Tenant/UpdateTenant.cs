namespace Fixawy.Areas.Admin.DTOS.Request.Tenant
{
    public class UpdateTenant
    {
        [MaxLength(1000)]
        public string? Name { get; set; } = string.Empty;
        [EmailAddress]
        public string? Email { get; set; } = string.Empty;
        public bool? HaveBranches { get; set; }
    }
}
