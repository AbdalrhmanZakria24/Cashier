using Microsoft.AspNetCore.Identity;

namespace Cashier.Application.CQRS.Command.Identity.ConfirmEmail
{
    public class ConfirmEmailCommandHandler
        : IRequestHandler<
            ConfirmEmailCommand,
            ResultT<bool>>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ConfirmEmailCommandHandler(
            UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ResultT<bool>> Handle(
            ConfirmEmailCommand request,
            CancellationToken cancellationToken)
        {
            var user =
                await _userManager.FindByIdAsync(request.Id);

            if (user == null)
            {
                return ResultT<bool>.Failure(
                    new Error(
                        "ConfirmEmail.UserNotFound",
                        "User not found",
                        ErrorType.NotFound));
            }

            if (user.EmailConfirmed)
                return ResultT<bool>.Success(true);
            

            var result =
                await _userManager.ConfirmEmailAsync(
                    user,
                    request.Token);

            if (!result.Succeeded)
            {
                var error = string.Join(
                    " ",
                    result.Errors.Select(x => x.Description));

                return ResultT<bool>.Failure(
                    new Error(
                        "ConfirmEmail.InvalidToken",
                        error,
                        ErrorType.Forbidden));
            }

            return ResultT<bool>.Success(true);
        }
    }
}