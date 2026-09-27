using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.AccessControl;

namespace Cashier.Application.CQRS.Command.Identity.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand,ResultT<string>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailSeder _emailSeder;

        public RegisterCommandHandler(
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor,
            IEmailSeder emailSeder)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _emailSeder = emailSeder;
        }

        public async Task<ResultT<string>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var checkUser = await _userManager.FindByEmailAsync(request.email);

            if (checkUser is not null)
                return ResultT<string>.Failure(new Error("Register.ElreadyExist",
                    "Email is already exist", ErrorType.Conflict));

            var newUser = new ApplicationUser(request.FullName, request.email, request.phoneNumber);


            var accountResult = await _userManager.CreateAsync(newUser, request.password);

            if (!accountResult.Succeeded)
            {
                var error = string.Join(
                    " ",
                    accountResult.Errors.Select(x => x.Description));

                return ResultT<string>.Failure(
                    new Error(
                        "Register.CannotCreateAccount",
                        error,
                        ErrorType.Forbidden));
            }

            var roleResult = await _userManager.AddToRoleAsync(newUser,Rl.Cashier);

            if (!roleResult.Succeeded)
            {
                var error = string.Join(
                    " ",
                    roleResult.Errors.Select(x => x.Description));

                return ResultT<string>.Failure(
                    new Error(
                        "Register.CannotAddRole",
                        error,
                        ErrorType.Forbidden));
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
            var tokenEncoded = Uri.EscapeDataString(token);

            var HttpRequest = _httpContextAccessor.HttpContext!.Request;
            var host = HttpRequest.Host.Value;
            var Scheme = HttpRequest.Scheme;

            var confirmationLink =
               $"{Scheme}://{host}" +
               $"/api/Identity/ConfirmEmail" +
               $"?token={tokenEncoded}" +
               $"&id={newUser.Id}";

            var htmlMessage = $"""
                <h2>Welcome to POS Cashier</h2>

                <p>Hello {newUser.fullName},</p>

                <p>
                    Thank you for registering.
                    Please confirm your email address
                    by clicking the button below.
                </p>

                <p>
                    <a href="{confirmationLink}">
                        Confirm Email
                    </a>
                </p>

                <p>
                    If you did not create this account,
                    you can ignore this email.
                </p>
                """;


            await _emailSeder.SendEmailAsync(
               newUser.Email!,
               "Confirm your email - POS Cashier",
               htmlMessage);

            if (!int.TryParse(newUser.Id, out var id))
            {
                return ResultT<string>.Failure(
                    new Error(
                        "Register.InvalidUserId",
                        "Invalid user id",
                        ErrorType.Failure));
            }

            return ResultT<string>.Success(newUser.Id);

        }
    }
}
