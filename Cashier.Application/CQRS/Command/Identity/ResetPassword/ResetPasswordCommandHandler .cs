using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cashier.Application.CQRS.Command.Identity.ResetPassword
{
    public class ResetPasswordCommandHandler: IRequestHandler<ResetPasswordCommand, ResultT<string>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public ResetPasswordCommandHandler(
            UserManager<ApplicationUser> userManager,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }
        public async Task<ResultT<string>> Handle(ResetPasswordCommand request,CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);

            if (user is null)
                return ResultT<string>.Failure(new Error("Identity.NotFound",
                   "not found this user", ErrorType.NotFound));

            var result = await _userManager.ResetPasswordAsync(
               user,
               request.ResetToken,
               request.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(
                    ", ",
                    result.Errors.Select(x => x.Description));

                return ResultT<string>
                    .Failure(new Error("Identity.NotFound",errors, ErrorType.Forbidden));
            }

            user.SecurityStamp = Guid.NewGuid().ToString();

            await _userManager.UpdateAsync(user);

            var userOtps = await _unitOfWork
                .ApplicationuserOtpReposatory
                .GetAsync(x =>
                    x.ApplicationUserId == request.UserId &&
                    x.IsValid);

            foreach (var otp in userOtps)
            {
                otp.IsValid = false;
            }

            await _unitOfWork.CommitAsync(cancellationToken);

            return ResultT<string>.Success(user.Id);
        }
    }
}
