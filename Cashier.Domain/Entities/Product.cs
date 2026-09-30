namespace Cashier.Domain.Entities
{
    public class Product
    {
        public long Id { get; set; }

        public long CategoryId { get; set; }
        public Category Category { get; set; } = null!;


        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public decimal Cost { get; set; }

        public bool IsActive { get; set; }

        public DateTime UpdatedAt { get; set; }
        public int Version { get; set; } = 1;

        public ICollection<BranchProduct> branchProducts { get; set; } = new List<BranchProduct>();

    }
}
