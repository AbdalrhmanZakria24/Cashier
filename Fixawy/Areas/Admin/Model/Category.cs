namespace Fixawy.Areas.Admin.Model
{
    public class Category
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public long TenantId { get; set; }
        public Tenant Tenants { get; set; } = null!;
    }
}
