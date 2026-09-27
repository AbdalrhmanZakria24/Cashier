using System;
using System.Collections.Generic;
using System.Text;

namespace Cashier.Application.DTO
{
    public class Pagination
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
