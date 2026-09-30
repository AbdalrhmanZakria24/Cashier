namespace Cashier.Application.CQRS.Query.Carts.GetCart
{
    public class GetCartQueryValidator
        : AbstractValidator<GetCartQuery>
    {
        public GetCartQueryValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty();
        }
    }
}
