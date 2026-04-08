namespace Fixawy.Areas.Admin.Model
{
    public class Product
    {
        public long Id { get; set; }

        public long TenantId { get; set; }
        public Tenant Tenant { get; set; } = null!;

        public long CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }
        public decimal Cost { get; set; }

        public bool IsActive { get; set; }

        public DateTime UpdatedAt { get; set; }
        public int Version { get; set; } = 1;

    }
}
