namespace Fixawy.Areas.Employee.DTOS.Response
{
    public class CartsResponse
    {
        public Guid Id { get; set; }
        public bool IsSuccess { get; set; }
        public string Massege { get; set; }=string.Empty;
        public DateTime CreatedAt { get; set; }

        public CartResponseDto data { get; set; } = null!;
    }
}
