namespace Fixawy.Areas.Identity.DTOS.Request
{
    public class ForgetPassword
    {
        [Required(ErrorMessage ="Enter your email or username")]
        public string EmailOrUsername { get; set; }=string.Empty;
    }
}
