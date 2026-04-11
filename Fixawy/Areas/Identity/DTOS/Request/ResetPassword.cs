namespace Fixawy.Areas.Identity.DTOS.Request
{
    public class ResetPassword
    {
        [Required(ErrorMessage ="enter your password")]
        [DataType(DataType.Password)]
        public string password { get; set; } =string.Empty;
        [DataType(DataType.Password)]
        [Compare(nameof(password))]
        [Required(ErrorMessage ="Enter confirm password")]
        public string confirmPassword { get; set; } =string.Empty;
    }
}
