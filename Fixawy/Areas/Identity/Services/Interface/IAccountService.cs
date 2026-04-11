namespace Fixawy.Areas.Identity.Services.Interface
{
    public interface IAccountService
    {
        public Task<IdentityResponse> Register(Register register, string Schema);
        public Task<IdentityResponse> ConfirmEmail(string token, string id);
        public  Task<IdentityResponse> Login(Login login);
        public Task<IdentityResponse> ForgetPassword(ForgetPassword forgetPassword, CancellationToken cancellationToken);
        public Task<IdentityResponse> ValidateOtp(ValidateOtp validateOtp, string id);
        public Task<IdentityResponse> ResetPassword(ResetPassword resetPassword, string ResetToken, string id);
    }
}
