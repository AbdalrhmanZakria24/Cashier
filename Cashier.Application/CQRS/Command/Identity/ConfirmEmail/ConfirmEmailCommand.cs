namespace Cashier.Application.CQRS.Command.Identity.ConfirmEmail
{
    public record ConfirmEmailCommand(
        string Id,
        string Token
    ) : IRequest<ResultT<bool>>;
}