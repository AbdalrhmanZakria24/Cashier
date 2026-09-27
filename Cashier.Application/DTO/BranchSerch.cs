using System.ComponentModel.DataAnnotations;

namespace Cashier.Application.DTO
{
    public class BranchSerch
    {
        [MaxLength(1000)]
        public string? Name { get; set; } = string.Empty;
        [MaxLength(1000)]
        public string? Address { get; set; } = string.Empty;

        public bool? IsActive { get; set; }
    }
}
