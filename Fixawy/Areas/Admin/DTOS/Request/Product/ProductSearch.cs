namespace Fixawy.Areas.Admin.DTOS.Request.Product
{
    public class ProductSearch
    {
        public long? CategoryId { get; set; }
        [MaxLength(1000)]
        public string? Name { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public bool? IsActive { get; set; }
        public long? TenantId { get; set; }
    }
}
