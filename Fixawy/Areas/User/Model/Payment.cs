namespace Fixawy.Areas.User.Model
{
    public enum PaymentMethod
    {
        wallet,
        visa,
    }
    public class Payment
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }

        public double Amount { get; set; }
        public DateTime PaymentDate { get; set; }

        public PaymentMethod PaymentMethod { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
