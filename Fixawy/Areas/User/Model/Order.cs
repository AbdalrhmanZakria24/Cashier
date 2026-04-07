namespace Fixawy.Areas.User.Model
{
    public enum OrderStatus
    {
        Pending,     
        Accepted,    
        OnTheWay,    
        Completed,   
        Cancelled
    }

    public class Order
    {
        public int Id { get; set; }

        public string UserId { get; set; } = string.Empty;
        public ApplicationUser ApplicationUser { get; set; }

        public int ServiceId { get; set; }
        public Category Service { get; set; }

        public string Description { get; set; } = string.Empty;

        public DateTime RequestTime { get; set; }

        public OrderStatus Status { get; set; }

        public double Price { get; set; }


    }
}
