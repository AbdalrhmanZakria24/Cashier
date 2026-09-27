using System;
using System.Collections.Generic;
using System.Text;

namespace Cashier.Application.DTO
{
    public class ProductResponse
    {
        public ICollection<Product> Data { get; set; } = new List<Product>();
        public int? totalCount { get; set; }
        public int? totalPages { get; set; }
        public int? currentPage { get; set; }
        public int? pageSize { get; set; }
    }
}
