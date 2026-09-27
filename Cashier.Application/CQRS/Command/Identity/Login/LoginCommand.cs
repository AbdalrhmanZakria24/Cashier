
namespace Cashier.Application.CQRS.Command.Identity.Login
{
    public sealed record LoginCommand(string Email,
        string Password,
        bool RememberMe) : IRequest<ResultT<LoginResponse>>;
}
