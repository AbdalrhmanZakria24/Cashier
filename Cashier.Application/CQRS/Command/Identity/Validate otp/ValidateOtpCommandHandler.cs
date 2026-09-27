using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cashier.Application.CQRS.Command.Identity.Validate_otp
{
    public class ValidateOtpCommandHandler: IRequestHandler<ValidateOtpCommand, ResultT<string>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public ValidateOtpCommandHandler(
            UserManager<ApplicationUser> userManager,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResultT<string>> Handle(ValidateOtpCommand request,CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);

            if (user is null)
            {
                return ResultT<string>.Failure(
                    new Error(
                        "Identity.ValidateOtp",
                        "User not found",
                        ErrorType.NotFound));
            }

            var matchOtp = (
                await _unitOfWork.ApplicationuserOtpReposatory.GetAsync(
                    x =>
                        x.ApplicationUserId == request.UserId &&
                        x.IsValid &&
                        x.ValidTo > DateTime.UtcNow &&
                        x.OtpNumber == request.Otp)
            ).FirstOrDefault();

            if (matchOtp is null)
            {
                return ResultT<string>.Failure(
                    new Error(
                        "Identity.ValidateOtp",
                        "Invalid or expired OTP",
                        ErrorType.Failure));
            }

            matchOtp.IsValid = false;

            _unitOfWork.ApplicationuserOtpReposatory.Update(matchOtp);

            await _unitOfWork.CommitAsync(cancellationToken);

            var resetToken =
                await _userManager.GeneratePasswordResetTokenAsync(user);

            return ResultT<string>.Success(resetToken);
        }
    }

}
