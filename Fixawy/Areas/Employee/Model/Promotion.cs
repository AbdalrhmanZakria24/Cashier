namespace Fixawy.Areas.Employee.Model
{
    public class Promotion
    {
        public long Id { get; set; }

        public string Code { get; set; } = string.Empty;

        public decimal? DiscountPercentage { get; set; }
        public decimal? DiscountAmount { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public long MaxUse { get; set; }

        public bool IsActive { get; set; }

        public long? ProductId { get; set; }
        public Product? Product { get; set; }
    }
}
