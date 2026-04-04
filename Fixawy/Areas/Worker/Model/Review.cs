namespace Fixawy.Areas.Worker.Model
{
    public class Review
    {
        public int Id { get; set; }

        public string UserId {  get; set; } = string.Empty;
        public ApplicationUser ApplicationUser { get; set; }


        public double Rate { get; set; }
        public string Comment { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}
