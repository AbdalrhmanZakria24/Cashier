namespace Fixawy.Areas.Identity.DTOS.Request
{
    public class Login
    {
        [Required(ErrorMessage ="Please enter email or username")]
        public string EmailOrUsername { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } =string.Empty;
        public bool RememberMe { get; set; } = true;
    }
}
