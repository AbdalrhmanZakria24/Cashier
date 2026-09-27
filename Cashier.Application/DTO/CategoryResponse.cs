using System;
using System.Collections.Generic;
using System.Text;

namespace Cashier.Application.DTO
{
    public class CategoryResponse
    {
        public ICollection<Category> Data { get; set; } = new List<Category>();
        public int? totalCount { get; set; }
        public int? totalPages { get; set; }
        public int? currentPage { get; set; }
        public int? pageSize { get; set; }
    }
}
