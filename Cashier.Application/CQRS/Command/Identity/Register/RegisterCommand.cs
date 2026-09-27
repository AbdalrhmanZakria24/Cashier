namespace Cashier.Application.CQRS.Command.Identity.Register
{
    public record RegisterCommand(string FullName,
        string email,
        string phoneNumber,
        string password,
        string ConfirmPassword) : IRequest<ResultT<string>>;
}
