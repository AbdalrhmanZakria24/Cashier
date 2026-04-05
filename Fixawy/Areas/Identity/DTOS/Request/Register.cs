using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace Fixawy.Areas.Identity.DTOS.Request
{
    public class Register
    {
        [Required(ErrorMessage ="Please enter your name")]
        public string Name { get; set; } =string.Empty;
        [Required(ErrorMessage = "Please enter your email")]
        public string email { get; set; } =string.Empty;

        [Required(ErrorMessage = "Pleace enter your phone number")]
        public string phoneNumber { get; set; } = string.Empty;
        [DataType(DataType.Password)]
        [Required(ErrorMessage ="Please enter your password")]
        public string password { get; set; } = string.Empty;
        [Compare(nameof(password))]
        [Required(ErrorMessage = "Please confirm your password")]
        public string ConfirmPassword { get; set; } = string.Empty;



    }
}
    