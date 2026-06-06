namespace Fixawy.Areas.Employee.DTOS.Response
{
    public class CartResponseDto
    {
        public long Id { get; set; }

        public decimal TotalAmount { get; set; }

        public int ItemsCount { get; set; }

        public List<CartItemDto> CartItems { get; set; }
    }
}
