namespace Cashier.Domain.Entities
{
    public class Branch
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public ICollection<BranchProduct> branchProducts { get; set; } = new List<BranchProduct>();
    }
}
