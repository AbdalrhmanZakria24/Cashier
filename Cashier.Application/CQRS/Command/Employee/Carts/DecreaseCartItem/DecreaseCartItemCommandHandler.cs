using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Cashier.Application.CQRS.Command.Employee.Carts.DecreaseCartItem
{
    public class DecreaseCartItemCommandHandler: IRequestHandler<DecreaseCartItemCommand,ResultT<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public DecreaseCartItemCommandHandler(
            IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<ResultT<bool>> Handle(
            DecreaseCartItemCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _userManager
               .FindByIdAsync(request.UserId);

            if (user is null)
            {
                return ResultT<bool>.Failure(
                    new Error(
                        "Cart.UserNotFound",
                        "User not found",
                        ErrorType.NotFound));
            }

            var product = await _unitOfWork.ProductReposatory
                .GetOneAsync(
                    x => x.Id == request.ProductId);

            if (product is null)
            {
                return ResultT<bool>.Failure(
                    new Error(
                        "Cart.ProductNotFound",
                        "Product not found",
                        ErrorType.NotFound));
            }
            var cart = await _unitOfWork.Cartreposatory
               .GetOneAsync(
                   x => x.ApplicationUserId == request.UserId,
                   new Expression<Func<Cart, object>>[]
                   {
                        x => x.CartItems
                   },
                   cancellationToken: cancellationToken);

            if (cart is null)
            {
                return ResultT<bool>.Failure(
                    new Error(
                        "Cart.NotFound",
                        "Cart not found",
                        ErrorType.NotFound));
            }

            var productItem = cart.CartItems
                .FirstOrDefault(
                    x => x.ProductId == request.ProductId);

            if (productItem is null)
            {
                return ResultT<bool>.Failure(
                    new Error(
                        "Cart.ItemNotFound",
                        "Product not in cart",
                        ErrorType.NotFound));
            }

            productItem.Quantity--;

            if (productItem.Quantity <= 0)
            {
                cart.CartItems.Remove(productItem);

                cart.TotalAmount =
                    cart.CartItems.Sum(x => x.Total);

                cart.ItemsCount =
                    cart.CartItems.Count;

                await _unitOfWork.CommitAsync(
                    cancellationToken);

                return ResultT<bool>.Success(true);
            }

            productItem.Total =
                productItem.Quantity *
                productItem.UnitPrice;

            cart.TotalAmount =
                cart.CartItems.Sum(x => x.Total);

            cart.ItemsCount =
                cart.CartItems.Count;

            await _unitOfWork.CommitAsync(
                cancellationToken);

            return ResultT<bool>.Success(true);
        }
    }
}
