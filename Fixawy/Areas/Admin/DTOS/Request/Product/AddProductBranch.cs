namespace Fixawy.Areas.Admin.DTOS.Request.Product
{
    public class AddProductBranch
    {
        [Range(1, long.MaxValue)]
        public long BranchId { get; set; }
        [Range(1, long.MaxValue)]
        public int Quantity { get; set; }
    }
}
