namespace Fixawy.Areas.Admin.DTOS.Request
{
    public class BranchSerch
    {
        [MaxLength(1000)]
        public string? Name { get; set; } = string.Empty;
        [MaxLength(1000)]
        public string? Address { get; set; } = string.Empty;

        public bool? IsActive { get; set; }
    }
}
