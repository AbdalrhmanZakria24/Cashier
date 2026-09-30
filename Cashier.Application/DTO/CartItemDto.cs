using System;
using System.Collections.Generic;
using System.Text;

namespace Cashier.Application.DTO
{
    public class CartItemDto
    {
        public long ProductId { get; set; }

        public decimal Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal Discount { get; set; }

        public decimal Total { get; set; }
    }
}
