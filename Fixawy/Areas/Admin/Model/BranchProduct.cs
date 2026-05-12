namespace Fixawy.Areas.Admin.Model
{
    public class BranchProduct
    {
        public long Id { get; set; }
        public long BranchId { get; set; }
        public Branch Branch { get; set; } = null!;

        public long ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public int Quantity { get; set; }

    }
}
