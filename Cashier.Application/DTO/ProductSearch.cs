using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Cashier.Application.DTO
{
    public class ProductSearch
    {
        public long? CategoryId { get; set; }
        [MaxLength(1000)]
        public string? Name { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public bool? IsActive { get; set; }
    }
}
