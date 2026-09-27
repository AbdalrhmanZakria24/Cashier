using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cashier.Application.CQRS.Command.Identity.ForgetPassword
{
    public class ForgetPasswordCommandHandler : IRequestHandler<ForgetPasswordCommand, ResultT<bool>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailSeder _emailSeder;
        private readonly ILogger<ForgetPasswordCommandHandler> _logger;

        public ForgetPasswordCommandHandler(
            UserManager<ApplicationUser> userManager,
            IUnitOfWork unitOfWork,
            IEmailSeder emailSeder,
            ILogger<ForgetPasswordCommandHandler> logger)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _emailSeder = emailSeder;
            _logger = logger;
        }

        public async Task<ResultT<bool>> Handle(ForgetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user =
               await _userManager.FindByEmailAsync(request.Email);

            if (user is null)
            {
                return ResultT<bool>.Failure(
                    new Error(
                        "Identity.ForgetPassword",
                        "Invalid  email",
                        ErrorType.NotFound));
            }

            var otpNumbers =
                await _unitOfWork.ApplicationuserOtpReposatory
                    .GetAsync(x => x.ApplicationUserId == user.Id);

            var count = otpNumbers.Count(x =>
                (DateTime.UtcNow - x.CreatedAt).TotalHours < 24);

            if (count >= 3)
            {
                foreach (var item in otpNumbers)
                {
                    if ((DateTime.UtcNow - item.CreatedAt).TotalHours < 24)
                    {
                        item.IsValid = false;
                    }
                }

                await _unitOfWork.CommitAsync(cancellationToken);

                return ResultT<bool>.Failure(
                    new Error(
                        "Identity.ForgetPassword",
                        "You tried too many attempts. Try again after 1 day.",
                        ErrorType.Failure));
            }

            var otpNumber = Random.Shared.Next(100000, 1000000).ToString();

            var createOtp = new ApplicationuserOtp
            {
                ApplicationUser = user,
                ApplicationUserId = user.Id,
                OtpNumber = otpNumber,
                CreatedAt = DateTime.UtcNow,
                ValidTo = DateTime.UtcNow.AddMinutes(5),
                IsValid = true
            };

            await _unitOfWork.ApplicationuserOtpReposatory
                .CreateAsync(createOtp, cancellationToken);

            await _unitOfWork.CommitAsync(cancellationToken);

            try
            {
                var html = GenerateOtpEmailHtml(
                    otpNumber,
                    user.UserName!);

                await _emailSeder.SendEmailAsync(
                    user.Email!,
                    "Reset Password OTP",
                    html);
            }

            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error sending reset password OTP email for user {UserId}",
                    user.Id);

                return ResultT<bool>.Failure(
                    new Error(
                        "Identity.ForgetPassword",
                        "Failed to send OTP email",
                        ErrorType.Failure));
            }

            return ResultT<bool>.Success(true);
        }

        private string GenerateOtpEmailHtml(
           string otpNumber,
           string userName)
        {
            return $"""
                <html>
                    <body>
                        <h2>Password Reset</h2>

                        <p>Hello {userName},</p>

                        <p>Your password reset OTP is:</p>

                        <h1>{otpNumber}</h1>

                        <p>This OTP will expire in 5 minutes.</p>

                        <p>If you did not request a password reset,
                        please ignore this email.</p>
                    </body>
                </html>
                """;
        }
    }
}
