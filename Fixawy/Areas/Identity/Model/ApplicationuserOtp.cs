namespace Fixawy.Areas.Identity.Model
{
    public class ApplicationuserOtp
    {
        public int Id { get; set; }
        public string ApplicationUserId {  get; set; } = string.Empty;
        public ApplicationUser ApplicationUser { get; set; } = null!;
        public string OtpNumber { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime ValidTo { get; set; }
        public bool IsValid { get; set; }
    }
}
