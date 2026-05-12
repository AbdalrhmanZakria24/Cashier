namespace Fixawy.Areas.Admin.Model
{
    public class Product
    {
        public long Id { get; set; }

        public long TenantId { get; set; }
        public Tenant Tenant { get; set; } = null!;

        public long CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        [Required]
        [MaxLength(1000)]
        public string Name { get; set; } = string.Empty;
        [Required]
        public decimal Price { get; set; }
        [Required]
        public decimal Cost { get; set; }
        [Required]
        public bool IsActive { get; set; }

        public DateTime UpdatedAt { get; set; }
        public int Version { get; set; } = 1;

        public ICollection<BranchProduct> branchProducts { get; set; } = new List<BranchProduct>();

    }
}
