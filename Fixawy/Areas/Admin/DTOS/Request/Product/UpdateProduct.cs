namespace Fixawy.Areas.Admin.DTOS.Request.Product
{
    public class UpdateProduct
    {
        public string? Name { get; set; }

        public decimal? Price { get; set; }

        public decimal? Cost { get; set; }

        public bool? IsActive { get; set; }

        public List<AddProductBranch>? AddProductBranches { get; set; }
    }
}
