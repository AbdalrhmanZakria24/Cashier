    namespace Fixawy.Areas.Employee.Model
{
    public class Invoice
    {
        public long Id { get; set; }

        public long TenantId { get; set; }

        public string InvoiceNumber { get; set; } = string.Empty;

        public long OrderId { get; set; }
        public Order Order { get; set; } = null!;

        public decimal SubTotal { get; set; }

        public decimal Discount { get; set; }

        public decimal Tax { get; set; }

        public decimal Total { get; set; }

        public DateTime IssuedAt { get; set; }

        public bool IsPaid { get; set; }
    }
}
