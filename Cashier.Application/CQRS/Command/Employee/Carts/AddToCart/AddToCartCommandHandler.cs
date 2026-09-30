using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Cashier.Application.CQRS.Command.Employee.Carts.AddToCart
{
    public class AddToCartCommandHandler
        : IRequestHandler<AddToCartCommand, ResultT<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public AddToCartCommandHandler(
            IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<ResultT<bool>> Handle(
           AddToCartCommand request,
           CancellationToken cancellationToken)
        {
            var user = await _userManager
               .FindByIdAsync(request.UserId);

            if (user is null)
            {
                return ResultT<bool>.Failure(
                    new Error(
                        "Cart.UserNotFound",
                        "User not found, please register first",
                        ErrorType.NotFound));
            }

            var product = await _unitOfWork.ProductReposatory
               .GetOneAsync(x => x.Id == request.ProductId);

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

            if (cart is not null)
            {
                var existingItem = cart.CartItems
                 .FirstOrDefault(
                     x => x.ProductId == product.Id);

                if (existingItem is not null)
                {
                    existingItem.Quantity += request.Quantity;

                    existingItem.Total =
                        existingItem.Quantity *
                        existingItem.UnitPrice;

                    cart.TotalAmount =
                        cart.CartItems.Sum(x => x.Total);

                    cart.ItemsCount =
                        cart.CartItems.Count;

                    await _unitOfWork.CommitAsync(
                        cancellationToken);

                    return ResultT<bool>.Success(true);
                }

                var cartItem = new CartItem
                {
                    CartId = cart.Id,
                    ProductId = product.Id,
                    Product = product,
                    Quantity = request.Quantity,
                    UnitPrice = product.Price,
                    Total = product.Price * request.Quantity
                };

                await _unitOfWork.CartItemreposatory
                    .CreateAsync(
                        cartItem,
                        cancellationToken);

                cart.CartItems.Add(cartItem);

                cart.TotalAmount =
                    cart.CartItems.Sum(x => x.Total);

                cart.ItemsCount =
                    cart.CartItems.Count;

                await _unitOfWork.CommitAsync(
                    cancellationToken);

                return ResultT<bool>.Success(true);
            }

            var newCart = new Cart
            {
                ApplicationUserId = request.UserId,
                IsActive = product.IsActive
            };

            await _unitOfWork.Cartreposatory
                .CreateAsync(
                    newCart,
                    cancellationToken);

            await _unitOfWork.CommitAsync(
                cancellationToken);

            var newCartItem = new CartItem
            {
                CartId = newCart.Id,
                ProductId = product.Id,
                Product = product,
                Quantity = request.Quantity,
                UnitPrice = product.Price,
                Total = product.Price * request.Quantity
            };

            await _unitOfWork.CartItemreposatory
                .CreateAsync(
                    newCartItem,
                    cancellationToken);

            newCart.CartItems.Add(newCartItem);

            newCart.TotalAmount =
                newCart.CartItems.Sum(x => x.Total);

            newCart.ItemsCount =
                newCart.CartItems.Count;

            await _unitOfWork.CommitAsync(
                cancellationToken);

            return ResultT<bool>.Success(true);
        }
    }
}
