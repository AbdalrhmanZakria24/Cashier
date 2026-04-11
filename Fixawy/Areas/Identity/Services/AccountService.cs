using Microsoft.AspNetCore.Identity;

namespace Fixawy.Areas.Identity.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<AccountService> _logger;
        private readonly IEmailSeder _emailSeder;
        private readonly ITokenService _tokenService;

        public AccountService(IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<AccountService> logger,
            IEmailSeder emailSeder,
            ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSeder = emailSeder;
            _tokenService = tokenService;
        }

        private string GenerateConfirmEmailHtml(string link, string userName)
        {
            return $@"
    <div style='font-family: Arial; background-color:#f4f4f4; padding:20px;'>
        <div style='max-width:600px; margin:auto; background:white; padding:30px; border-radius:10px; text-align:center;'>
            
            <h2 style='color:#333;'>Welcome {userName} 👋</h2>
            
            <p style='color:#555; font-size:16px;'>
                Thank you for registering in our POS system.
                Please confirm your email to activate your account.
            </p>

            <a href='{link}' 
               style='display:inline-block; margin-top:20px; padding:12px 25px; 
                      background-color:#007bff; color:white; text-decoration:none; 
                      border-radius:5px; font-size:16px;'>
                Confirm Email
            </a>

            <p style='margin-top:30px; font-size:12px; color:#999;'>
                If you did not create this account, please ignore this email.
            </p>

        </div>
    </div>";
        }
        private string GenerateOtpEmailHtml(string otp, string userName)
        {
            return $@"
<!DOCTYPE html>
<html>
<body style='margin:0; background:#f4f6f8; font-family:Arial;'>

<table width='100%' style='padding:20px;'>
<tr>
<td align='center'>

<table width='600' style='background:#fff; border-radius:10px; padding:30px; text-align:center;'>

<tr>
<td style='background:#007bff; padding:20px; border-radius:10px 10px 0 0;'>
    <h2 style='color:white;'>Fixawy Security</h2>
</td>
</tr>

<tr>
<td style='padding:30px;'>

    <h3>Hello {userName} 👋</h3>

    <p style='color:#555; font-size:15px;'>
        We received a request to reset your password.
    </p>

    <p style='color:#555; font-size:15px;'>
        Use the OTP below to continue:
    </p>

    <div style='font-size:32px; font-weight:bold; 
                letter-spacing:5px; margin:20px 0; color:#007bff;'>
        {otp}
    </div>

    <p style='color:#999; font-size:13px;'>
        This code will expire in 5 minutes.
    </p>

    <hr style='margin:25px 0;' />

    <p style='color:#aaa; font-size:12px;'>
        If you didn’t request this, you can ignore this email.
    </p>

</td>
</tr>

<tr>
<td style='background:#f9f9f9; padding:10px; font-size:12px; color:#aaa;'>
    © {DateTime.UtcNow.Year}  POS
</td>
</tr>

</table>

</td>
</tr>
</table>

</body>
</html>";
        }

        public async Task<IdentityResponse> Register(Register register, string Schema)
        {
            var checkEmail = await _userManager.FindByEmailAsync(register.email);
            var checkUserName = await _userManager.FindByNameAsync(register.UserName);

            if (checkEmail is not null || checkUserName is not null)
            {
                return new IdentityResponse
                {
                    isSuccess = false,
                    message = "Email or username already exists",
                    createdAt = DateTime.UtcNow
                };
            }

            var newUser = new ApplicationUser
            {
                UserName = register.UserName,
                fullName = register.UserName,
                Email = register.email,
                PhoneNumber = register.phoneNumber
            };

            var result = await _userManager.CreateAsync(newUser, register.password);

            if (!result.Succeeded)
            {

                var error = string.Join(",", result.Errors.Select(e => e.Description));

                return new IdentityResponse
                {
                    isSuccess = false,
                    message = error,
                    createdAt = DateTime.UtcNow
                };
            }

            await _userManager.AddToRoleAsync(newUser, Rl.Customer);

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
            var tokenEncoded = Uri.EscapeDataString(token);

            var link = $"{Schema}://localhost:7210/Identity/Account/ConfirmEmail?token={tokenEncoded}&id={newUser.Id}";

            var htmlMessage = GenerateConfirmEmailHtml(link, newUser.UserName);

            try
            {
                await _emailSeder.SendEmailAsync(newUser.Email, "Confirm your account", htmlMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Email error: {ex.Message}");

                return new IdentityResponse
                {
                    isSuccess = true,
                    message = "Account created, but failed to send confirmation email.",
                    createdAt = DateTime.UtcNow
                };
            }

            return new IdentityResponse
            {
                isSuccess = true,
                message = "Please check your email to confirm your account.",
                createdAt = DateTime.UtcNow
            };
        }

        public async Task<IdentityResponse> ConfirmEmail(string token, string id)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(token))
                return new IdentityResponse()
                {
                    isSuccess = false,
                    message = "Invalid confirmation request ",
                    createdAt = DateTime.UtcNow
                };

            var user = await _userManager.FindByIdAsync(id);

            if (user is null)
            {
                return new IdentityResponse
                {
                    isSuccess = false,
                    message = "User not found",
                    createdAt = DateTime.UtcNow
                };
            }

            if (user.EmailConfirmed)
            {
                return new IdentityResponse
                {
                    isSuccess = true,
                    message = "Email is already confirmed",
                    createdAt = DateTime.UtcNow
                };
            }

            token = Uri.UnescapeDataString(token);

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (!result.Succeeded)
            {

                return new IdentityResponse
                {
                    isSuccess = false,
                    message = "Invalid or expired confirmation link",
                    createdAt = DateTime.UtcNow
                };
            }

            return new IdentityResponse
            {
                isSuccess = true,
                message = "Email confirmed successfully",
                createdAt = DateTime.UtcNow
            };

        }

        public async Task<IdentityResponse> Login(Login login)
        {
            var user = await _userManager.FindByEmailAsync(login.EmailOrUsername) ??
                await _userManager.FindByNameAsync(login.EmailOrUsername);

            if (user is null)
            {
                return new IdentityResponse
                {
                    isSuccess = false,
                    message = "Invalid email or username",
                    createdAt = DateTime.UtcNow
                };
            }

            if (!user.EmailConfirmed)
            {
                return new IdentityResponse
                {
                    isSuccess = false,
                    message = "confirm your email first",
                    createdAt = DateTime.UtcNow
                };
            }

            var result = await _signInManager.PasswordSignInAsync(user, login.Password, login.RememberMe, true);

            if (result.IsLockedOut)
            {
                return new IdentityResponse
                {
                    isSuccess = false,
                    message = "Account locked. Try again later",
                    createdAt = DateTime.UtcNow
                };
            }

            if (!result.Succeeded)
            {
                return new IdentityResponse
                {
                    isSuccess = false,
                    message = "Invalid username or password",
                    createdAt = DateTime.UtcNow
                };
            }

            var AccessToken = await _tokenService.GenerateJwtTokenAsync(user);
            var RefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = RefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            await _userManager.UpdateAsync(user);

            return new IdentityResponse
            {
                isSuccess = true,
                message = "Login successfully",
                createdAt = DateTime.UtcNow,
                AccessToken = AccessToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(30),
                RefreshToken = RefreshToken,
                RefreshTokenExpiryTime = user.RefreshTokenExpiryTime,
            };
        }

        public async Task<IdentityResponse> ForgetPassword(ForgetPassword forgetPassword, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(forgetPassword.EmailOrUsername))
            {
                return new IdentityResponse
                {
                    isSuccess = false,
                    message = "enter your email or username",
                    createdAt = DateTime.UtcNow
                };
            }

            var user = await _userManager.FindByEmailAsync(forgetPassword.EmailOrUsername) ??
                await _userManager.FindByNameAsync(forgetPassword.EmailOrUsername);

            if (user is null)
            {
                return new IdentityResponse
                {
                    isSuccess = false,
                    message = "Invalid username or email",
                    createdAt = DateTime.UtcNow
                };
            }

            var otpNumber = await _unitOfWork.ApplicationuserOtpReposatory.GetAsync(s => s.ApplicationUserId == user.Id);

            var count = otpNumber.Count(s => (DateTime.UtcNow - s.CreatedAt).TotalHours < 24);

            if (count > 3)
            {
                foreach (var item in otpNumber)
                    item.IsValid = false;

                return new IdentityResponse
                {
                    isSuccess = false,
                    message = "You tried too many attempts. Try again after 1 day.",
                    createdAt = DateTime.UtcNow
                };
            }



            var newOtpNumber = new Random().Next(100000, 999999).ToString();

            var CreateOtp = new ApplicationuserOtp
            {
                ApplicationUser = user,
                ApplicationUserId = user.Id,
                OtpNumber = newOtpNumber,
                CreatedAt = DateTime.UtcNow,
                ValidTo = DateTime.UtcNow.AddMinutes(5),
                IsValid = true
            };

            await _unitOfWork.ApplicationuserOtpReposatory.CreateAsync(CreateOtp, cancellationToken);

            await _unitOfWork.CommitAsync();

            try
            {
                var html = GenerateOtpEmailHtml(newOtpNumber, user.UserName!);

                await _emailSeder.SendEmailAsync(user.Email!,
                    "Reset Password OTP",
                   html);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error {ex.Message}");

                return new IdentityResponse
                {
                    isSuccess = false,
                    message = ex.Message,
                    createdAt = DateTime.UtcNow
                };
            }

            return new IdentityResponse
            {
                isSuccess = true,
                message = "If the account exists, an OTP has been sent",
                createdAt = DateTime.UtcNow
            };
        }

        public async Task<IdentityResponse> ValidateOtp(ValidateOtp validateOtp, string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user is null)
            {
                return new IdentityResponse
                {
                    isSuccess = false,
                    message = "user not allow",
                    createdAt = DateTime.UtcNow
                };
            }

            var matchOtp = (await _unitOfWork.ApplicationuserOtpReposatory
                .GetAsync(x =>
                      x.ApplicationUserId == id &&
                      x.IsValid &&
                      x.ValidTo > DateTime.UtcNow &&
                      x.OtpNumber == validateOtp.otp
                        )).FirstOrDefault();

            if (matchOtp is null)
            {
                return new IdentityResponse
                {
                    isSuccess = false,
                    message = "Not valid otp number",
                    createdAt = DateTime.UtcNow
                };
            }

            matchOtp.IsValid = false;

            _unitOfWork.ApplicationuserOtpReposatory.Update(matchOtp);

            await _unitOfWork.CommitAsync();

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            return new IdentityResponse
            {
                isSuccess = true,
                message = "Otp is valid",
                createdAt = DateTime.UtcNow,
                ResetToken = resetToken
            };
        }

        public async Task<IdentityResponse> ResetPassword(ResetPassword resetPassword,string ResetToken,string id)
        {
            if (resetPassword.password is null)
            {
                return new IdentityResponse()
                {
                    isSuccess = false,
                    message = "you must enter new password"
                };
            }

            var user =await _userManager.FindByIdAsync(id);

            if(user is null)
            {
                return new IdentityResponse
                {
                    isSuccess = false,
                    message = "user not allow",
                    createdAt = DateTime.UtcNow
                };
            }

            var result = await _userManager.ResetPasswordAsync(user, ResetToken, resetPassword.password);

            if (!result.Succeeded)
            {
                return new IdentityResponse()
                {
                    isSuccess = false,
                    message = "Cannot reset password"
                };
            }

            user.SecurityStamp = Guid.NewGuid().ToString();
            await _userManager.UpdateAsync(user);

            var userOtp = await _unitOfWork.ApplicationuserOtpReposatory
                .GetAsync(x => x.ApplicationUserId == id &&
                x.IsValid);

            foreach (var otp in userOtp)
                otp.IsValid = false;

            await _unitOfWork.CommitAsync();

            return new IdentityResponse()
            {
                isSuccess = true,
                message = "password reset success"
            };

        }


    }
}
