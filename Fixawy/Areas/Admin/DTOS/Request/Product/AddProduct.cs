namespace Fixawy.Areas.Admin.DTOS.Request.Product
{
    public class AddProduct
    {
        [Required]
        public long TenantId { get; set; }
        [Required]
        public long CategoryId { get; set; }
        [Required]
        [MaxLength(1000)]
        public string Name { get; set; } = string.Empty;
        [Required]
        public decimal Price { get; set; }
        [Required]
        public decimal Cost { get; set; }
        [Required]
        public bool IsActive { get; set; }
        public int Version { get; set; } = 1;
        public List<AddProductBranch> AddProductBranches { get; set; } = new List<AddProductBranch>();
    }
}
