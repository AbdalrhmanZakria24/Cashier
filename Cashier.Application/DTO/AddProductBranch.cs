using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Cashier.Application.DTO
{
    public class AddProductBranch
    {
        [Range(1, long.MaxValue)]
        public long BranchId { get; set; }
        [Range(1, long.MaxValue)]
        public int Quantity { get; set; }
    }
}
