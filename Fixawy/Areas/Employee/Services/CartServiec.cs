using Fixawy.Areas.Employee.DTOS.Response;
using Fixawy.DTOS.Response;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Storage.Json;
using System.Linq.Expressions;

namespace Fixawy.Areas.Employee.Services
{
    public class CartServiec : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartServiec(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<CartsResponse> GetCart(string Userid, string? Code, CancellationToken cancellationToken)
        {
            var CheckUser = await _userManager.FindByIdAsync(Userid);

            if (CheckUser is null)
            {
                return new CartsResponse()
                {
                    IsSuccess = false,
                    Massege = "User Id is not found",
                    CreatedAt = DateTime.UtcNow,
                };
            }

            Promotion? promoCode = null;

            if (!string.IsNullOrEmpty(Code))
            {

                promoCode = await _unitOfWork.Promotionreposatory.GetOneAsync(c => c.Code == Code);

                if (promoCode is null)
                {
                    return new CartsResponse()
                    {
                        IsSuccess = false,
                        Massege = "Not found this code"
                    };
                }

                if (!promoCode.IsActive || promoCode.StartDate >= DateTime.UtcNow || promoCode.EndDate < DateTime.UtcNow || promoCode.MaxUse <= 0)
                {
                    return new CartsResponse()
                    {
                        IsSuccess = false,
                        Massege = "Code not valid"
                    };
                }
            }

            Cart? cart;

            if (promoCode is not null)
            {
                cart = await _unitOfWork.Cartreposatory
                    .GetOneAsync(
               c => c.ApplicationUserId == Userid,
               include: new Expression<Func<Cart, object>>[]
               {
                    c => c.CartItems
               },
               cancellationToken: cancellationToken);
            }
            else
            {
                cart = await _unitOfWork.Cartreposatory
                    .GetOneAsync(
               c => c.ApplicationUserId == Userid,
               include: new Expression<Func<Cart, object>>[]
               {
                    c => c.CartItems,
               },
               cancellationToken: cancellationToken);
            }

            if (cart is null)
            {
                return new CartsResponse()
                {
                    IsSuccess = false,
                    Massege = "Cart is Empty",
                    CreatedAt = DateTime.UtcNow,
                };
            }


            if (promoCode is not null)
            {
                var targetItem = cart.CartItems
                    .FirstOrDefault(x => x.ProductId == promoCode.ProductId);

                if (targetItem is null)
                {
                    return new CartsResponse()
                    {
                        IsSuccess = false,
                        Massege = "Promo not applicable",
                        CreatedAt = DateTime.UtcNow,
                    };
                }

                if (targetItem.Discount == 0)
                {
                    decimal discount = (targetItem.UnitPrice * targetItem.Quantity * promoCode.DiscountPercentage!.Value) / 100;

                    targetItem.Discount = discount;
                    targetItem.Total = (targetItem.UnitPrice * targetItem.Quantity) - discount;
                    cart.TotalAmount = cart.CartItems.Sum(x => x.Total);

                    promoCode.MaxUse--;
                    await _unitOfWork.CommitAsync();
                }
            }

            var cartDto = new CartResponseDto
            {
                Id = cart.Id,

                TotalAmount = cart.TotalAmount,

                ItemsCount = cart.ItemsCount,

                CartItems = cart.CartItems.Select(x => new CartItemDto
                {
                    ProductId = x.Id,
                    Quantity = x.Quantity,
                    UnitPrice = x.UnitPrice,
                    Discount = x.Discount,
                    Total = x.Total

                }).ToList()
            };

            return new CartsResponse()
            {
                IsSuccess = true,
                Massege = "All product in cart",
                CreatedAt = DateTime.UtcNow,
                data = cartDto
            };
        }

        public async Task<AdminResponse> AddInCart(long ProductId, string UserId, decimal Quantity, CancellationToken cancellationToken)
        {
            var usetrid = await _userManager.FindByIdAsync(UserId);

            if (usetrid is null)
            {
                return new AdminResponse()
                {
                    isSuccess = false,
                    message = "User not found ,please register firet",
                    createAt = DateTime.UtcNow,
                };
            }

            var product = await _unitOfWork.ProductReposatory
                .GetOneAsync(x => x.Id == ProductId);

            if (product is null)
            {
                return new AdminResponse()
                {
                    isSuccess = false,
                    message = "Product not found",
                    createAt = DateTime.UtcNow,
                };
            }

            var CheckProduct = await _unitOfWork.Cartreposatory
              .GetOneAsync(x => x.ApplicationUserId == UserId,
              new Expression<Func<Cart, object>>[]
              {
                        c=>c.CartItems
              }, cancellationToken: cancellationToken);

            var CartItem = new CartItem();

            if (CheckProduct is not null)
            {
                if (CheckProduct.CartItems.Any(x => x.ProductId == product.Id))
                {
                    var item = CheckProduct.CartItems
                        .First(x => x.ProductId == product.Id);

                    item.Quantity += Quantity;

                    item.Total = (item.Quantity * item.UnitPrice);

                    CheckProduct.TotalAmount += CartItem.Total;

                    await _unitOfWork.CommitAsync();

                    CheckProduct.TotalAmount = CheckProduct.CartItems.Sum(x => x.Total);
                    CheckProduct.ItemsCount = CheckProduct.CartItems.Count();

                    await _unitOfWork.CommitAsync();


                    return new AdminResponse()
                    {
                        isSuccess = true,
                        message = "Add Success for cart",
                        createAt = DateTime.UtcNow
                    };
                }

                CartItem = new CartItem()
                {
                    CartId = CheckProduct.Id,
                    ProductId = product.Id,
                    Product = product,
                    Quantity = Quantity,
                    UnitPrice = product.Price,
                    Total = (product.Price * Quantity)
                };

                await _unitOfWork.CartItemreposatory.CreateAsync(CartItem, cancellationToken);

                CheckProduct.CartItems.Add(CartItem);

                CheckProduct.TotalAmount = CheckProduct.CartItems.Sum(x => x.Total);
                CheckProduct.ItemsCount = CheckProduct.CartItems.Count();

                await _unitOfWork.CommitAsync();

                return new AdminResponse()
                {
                    isSuccess = true,
                    message = "Add Success for cart",
                    createAt = DateTime.UtcNow
                };
            }

            var Cart = new Cart();

            try
            {
                Cart = new Cart
                {
                    TenantId = product.TenantId,
                    ApplicationUserId = UserId,
                    IsActive = product.IsActive,
                };

                await _unitOfWork.Cartreposatory.CreateAsync(Cart, cancellationToken);

                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                return new AdminResponse()
                {
                    isSuccess = false,
                    message = ex.Message,
                    createAt = DateTime.UtcNow,
                };
            }


            CartItem = new CartItem()
            {
                CartId = Cart.Id,
                ProductId = product.Id,
                Product = product,
                Quantity = Quantity,
                UnitPrice = product.Price,
                Total = (product.Price * Quantity)
            };

            await _unitOfWork.CartItemreposatory.CreateAsync(CartItem, cancellationToken);
            await _unitOfWork.CommitAsync();

            Cart.CartItems.Add(CartItem);

            Cart.TotalAmount = Cart.CartItems.Sum(x => x.Total);
            Cart.ItemsCount = Cart.CartItems.Count();

            await _unitOfWork.CommitAsync();

            return new AdminResponse()
            {
                isSuccess = true,
                message = "Add Success for cart",
                createAt = DateTime.UtcNow
            };
        }

        public async Task<AdminResponse> Increase(long ProductId, string UserId, CancellationToken cancellationToken)
        {
            var usetrid = await _userManager.FindByIdAsync(UserId);

            if (usetrid is null)
            {
                return new AdminResponse()
                {
                    isSuccess = false,
                    message = "User not found ,please register firet",
                    createAt = DateTime.UtcNow,
                };
            }

            var product = await _unitOfWork.ProductReposatory
                .GetOneAsync(x => x.Id == ProductId);

            if (product is null)
            {
                return new AdminResponse()
                {
                    isSuccess = false,
                    message = "Product not found",
                    createAt = DateTime.UtcNow,
                };
            }

            var CheckProduct = await _unitOfWork.Cartreposatory
              .GetOneAsync(x => x.ApplicationUserId == UserId,
              new Expression<Func<Cart, object>>[]
              {
                        c=>c.CartItems
              }, cancellationToken: cancellationToken);

            if (CheckProduct is null)
            {
                return new AdminResponse()
                {
                    isSuccess = false,
                    message = "product not found in cart",
                    createAt = DateTime.UtcNow,
                };
            }

            var productItem = CheckProduct.CartItems.FirstOrDefault(c => c.ProductId == ProductId);

            if (productItem is null)
            {
                return new AdminResponse
                {
                    isSuccess = false,
                    message = "Product not in cart , Please add product in cart",
                    createAt = DateTime.UtcNow
                };
            }

            productItem.Quantity++;

            productItem.Total = (productItem.Quantity * productItem.UnitPrice);

            CheckProduct.TotalAmount = CheckProduct.CartItems.Sum(c => c.Total);

            await _unitOfWork.CommitAsync();

            return new AdminResponse()
            {
                isSuccess = true,
                message = "Done",
                createAt = DateTime.UtcNow,
            };
        }

        public async Task<AdminResponse> Decrease(long ProductId, string UserId, CancellationToken cancellationToken)
        {
            var usetrid = await _userManager.FindByIdAsync(UserId);

            if (usetrid is null)
            {
                return new AdminResponse()
                {
                    isSuccess = false,
                    message = "User not found ,please register firet",
                    createAt = DateTime.UtcNow,
                };
            }

            var product = await _unitOfWork.ProductReposatory
                .GetOneAsync(x => x.Id == ProductId);

            if (product is null)
            {
                return new AdminResponse()
                {
                    isSuccess = false,
                    message = "Product not found",
                    createAt = DateTime.UtcNow,
                };
            }

            var CheckProduct = await _unitOfWork.Cartreposatory
              .GetOneAsync(x => x.ApplicationUserId == UserId,
              new Expression<Func<Cart, object>>[]
              {
                        c=>c.CartItems
              }, cancellationToken: cancellationToken);

            if (CheckProduct is null)
            {
                return new AdminResponse()
                {
                    isSuccess = false,
                    message = "product not found in cart",
                    createAt = DateTime.UtcNow,
                };
            }

            var productItem = CheckProduct.CartItems.FirstOrDefault(c => c.ProductId == ProductId);

            if (productItem is null)
            {
                return new AdminResponse
                {
                    isSuccess = false,
                    message = "Product not in cart , Please add product in cart",
                    createAt = DateTime.UtcNow
                };
            }

            if (productItem.Quantity <= 0)
            {
                CheckProduct.CartItems.Remove(productItem);

                CheckProduct.TotalAmount = CheckProduct.CartItems.Sum(c => c.Total);

                CheckProduct.ItemsCount = CheckProduct.CartItems.Count();

                await _unitOfWork.CommitAsync();

                return new AdminResponse()
                {
                    isSuccess = true,
                    message = "product  is removed",
                    createAt = DateTime.UtcNow,
                };
            }

            productItem.Quantity--;

            productItem.Total = (productItem.Quantity * productItem.UnitPrice);

            CheckProduct.TotalAmount = CheckProduct.CartItems.Sum(c => c.Total);
            CheckProduct.ItemsCount = CheckProduct.CartItems.Count();

            await _unitOfWork.CommitAsync();

            return new AdminResponse()
            {
                isSuccess = true,
                message = "Done",
                createAt = DateTime.UtcNow,
            };
        }

        public async Task<AdminResponse> Remove(long ProductId, string UserId, CancellationToken cancellationToken)
        {
            var usetrid = await _userManager.FindByIdAsync(UserId);

            if (usetrid is null)
            {
                return new AdminResponse()
                {
                    isSuccess = false,
                    message = "User not found ,please register firet",
                    createAt = DateTime.UtcNow,
                };
            }

            var product = await _unitOfWork.ProductReposatory
                .GetOneAsync(x => x.Id == ProductId);

            if (product is null)
            {
                return new AdminResponse()
                {
                    isSuccess = false,
                    message = "Product not found",
                    createAt = DateTime.UtcNow,
                };
            }

            var CheckProduct = await _unitOfWork.Cartreposatory
              .GetOneAsync(x => x.ApplicationUserId == UserId,
              new Expression<Func<Cart, object>>[]
              {
                        c=>c.CartItems
              }, cancellationToken: cancellationToken);

            if (CheckProduct is null)
            {
                return new AdminResponse()
                {
                    isSuccess = false,
                    message = "product not found in cart",
                    createAt = DateTime.UtcNow,
                };
            }

            var productItem = CheckProduct.CartItems.FirstOrDefault(c => c.ProductId == ProductId);

            if (productItem is null)
            {
                return new AdminResponse
                {
                    isSuccess = false,
                    message = "Product not in cart , Please add product in cart",
                    createAt = DateTime.UtcNow
                };
            }

            CheckProduct.CartItems.Remove(productItem);

            CheckProduct.TotalAmount = CheckProduct.CartItems.Sum(c => c.Total);

            CheckProduct.ItemsCount = CheckProduct.CartItems.Count();

            await _unitOfWork.CommitAsync();

            return new AdminResponse()
            {
                isSuccess = true,
                message = "product  is removed",
                createAt = DateTime.UtcNow,
            };
        }
    }
}

