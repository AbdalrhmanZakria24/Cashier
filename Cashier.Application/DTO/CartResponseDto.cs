using System;
using System.Collections.Generic;
using System.Text;

namespace Cashier.Application.DTO
{
    public class CartResponseDto
    {
        public long Id { get; set; }

        public decimal TotalAmount { get; set; }

        public int ItemsCount { get; set; }

        public List<CartItemDto> CartItems { get; set; }
    }
}
