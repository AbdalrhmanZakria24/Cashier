namespace Cashier.Domain.Entities
{
    public enum PaymentTransactionStatus
    {
        Initiated,   
        Success,     
        Failed,      
        Pending      
    }
    public class PaymentTransaction
    {
        public long Id { get; set; }

        public long PaymentId { get; set; }
        public Payment Payment { get; set; } = null!;

        public PaymentTransactionStatus Status { get; set; }
        public string? ResponseMessage { get; set; }
        public string? GatewayResponse { get; set; }
        public string? TransactionReference { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
