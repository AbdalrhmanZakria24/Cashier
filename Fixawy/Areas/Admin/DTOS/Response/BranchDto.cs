namespace Fixawy.Areas.Admin.DTOS.Response
{
    public class BranchDto
    {
        public string BranchName { get; set; } =string.Empty;
        public string Address { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public string TenantName { get; set; } = string.Empty;
        public string TenantEmail { get; set; } = string.Empty;
    }
}
