using Microsoft.AspNetCore.Identity;
using System.Linq.Expressions;

namespace Cashier.Application.CQRS.Query.Carts.GetCart
{
    public class GetCartQueryHandler
        : IRequestHandler<GetCartQuery, ResultT<CartResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public GetCartQueryHandler(
            IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<ResultT<CartResponseDto>> Handle(
            GetCartQuery request,
            CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);

            if (user is null)
            {
                return ResultT<CartResponseDto>.Failure(
                    new Error(
                        "Cart.UserNotFound",
                        "User Id is not found",
                        ErrorType.NotFound));
            }

            Promotion? promoCode = null;

            if (!string.IsNullOrWhiteSpace(request.Code))
            {
                promoCode = await _unitOfWork.Promotionreposatory
                    .GetOneAsync(
                        x => x.Code == request.Code);

                if (promoCode is null)
                {
                    return ResultT<CartResponseDto>.Failure(
                        new Error(
                            "Cart.PromoNotFound",
                            "Promo code not found",
                            ErrorType.NotFound));
                }

                if (!promoCode.IsActive ||
                    promoCode.StartDate >= DateTime.UtcNow ||
                    promoCode.EndDate < DateTime.UtcNow ||
                    promoCode.MaxUse <= 0)
                {
                    return ResultT<CartResponseDto>.Failure(
                        new Error(
                            "Cart.InvalidPromo",
                            "Promo code is not valid",
                            ErrorType.Validation));
                }
            }

            var cart = await _unitOfWork.Cartreposatory
                .GetOneAsync(
                    x => x.ApplicationUserId == request.UserId,
                    include: new Expression<Func<Cart, object>>[]
                    {
                        x => x.CartItems
                    },
                    cancellationToken: cancellationToken);

            if (cart is null)
            {
                return ResultT<CartResponseDto>.Failure(
                    new Error(
                        "Cart.NotFound",
                        "Cart is empty",
                        ErrorType.NotFound));
            }

            if (promoCode is not null)
            {
                var targetItem = cart.CartItems
                    .FirstOrDefault(
                        x => x.ProductId == promoCode.ProductId);

                if (targetItem is null)
                {
                    return ResultT<CartResponseDto>.Failure(
                        new Error(
                            "Cart.PromoNotApplicable",
                            "Promo is not applicable to this cart",
                            ErrorType.Validation));
                }

                if (targetItem.Discount == 0)
                {
                    var discount =
                        (targetItem.UnitPrice *
                         targetItem.Quantity *
                         promoCode.DiscountPercentage!.Value) / 100;

                    targetItem.Discount = discount;

                    targetItem.Total =
                        (targetItem.UnitPrice *
                         targetItem.Quantity) - discount;

                    cart.TotalAmount =
                        cart.CartItems.Sum(x => x.Total);

                    promoCode.MaxUse--;

                    await _unitOfWork.CommitAsync(
                        cancellationToken);
                }
            }

            var response = new CartResponseDto
            {
                Id = cart.Id,

                TotalAmount = cart.TotalAmount,

                ItemsCount = cart.ItemsCount,

                CartItems = cart.CartItems
                    .Select(x => new CartItemDto
                    {
                        ProductId = x.Id,
                        Quantity = x.Quantity,
                        UnitPrice = x.UnitPrice,
                        Discount = x.Discount,
                        Total = x.Total
                    })
                    .ToList()
            };

            return ResultT<CartResponseDto>.Success(response);
        }
    }
}