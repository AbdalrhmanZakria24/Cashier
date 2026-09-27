using Fixawy.DTOS.Response;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;
using System.Linq.Expressions;

namespace Fixawy.Areas.Employee.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public PaymentService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<AdminResponse> CreateStripePaymentIntent( string userId, CancellationToken cancellationToken)
        {
            var CheckuserId = await _userManager.FindByIdAsync(userId);

            if (CheckuserId == null)
            {
                return new AdminResponse()
                {
                    isSuccess = false,
                    message = "User not found",
                    createAt = DateTime.UtcNow,
                };
            }

            var cart = await _unitOfWork.Cartreposatory
                .GetOneAsync(x => x.ApplicationUserId == userId, new Expression<Func<Cart, object>>[]
                {
                    c=>c.CartItems,
                    c=>c.CartItems.Select(i=>i.Product)
                });

            if (cart == null || !cart.CartItems.Any())
            {
                return new AdminResponse
                {
                    isSuccess = false,
                    message = "Cart is empty",
                    createAt = DateTime.UtcNow
                };
            }

            var total = cart.CartItems.Sum(i => i.Total);

            var payment = new Payment
            {
                cartId = cart.Id,
                TenantId = cart.TenantId,
                ApplicationUserId = userId,
                Amount = total,
                Status = PaymentStatus.Pending,
                Method = Model.PaymentMethod.Card
            };

            await _unitOfWork.Paymentreposatory.CreateAsync(payment, cancellationToken);
            await _unitOfWork.CommitAsync();

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                Mode = "payment",
                LineItems = new List<SessionLineItemOptions>(),
                SuccessUrl = "https://localhost:7210/Employee/Payment/success?session_id={CHECKOUT_SESSION_ID}",
                CancelUrl = "https://localhost:7210/Employee/payment/cancel"
            };

            foreach (var item in cart.CartItems)
            {
                options.LineItems.Add(new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "egp",
                        UnitAmount = (long)(item.Total * 100),
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Name
                        }
                    },
                   // Quantity = item.Quantity
                });
            }

            var service = new SessionService();
            var session = service.Create(options);


            payment.TransactionId = session.Id; 
            await _unitOfWork.CommitAsync();

            return new AdminResponse
            {
                isSuccess = true,
                message = session.Url,
                createAt = DateTime.UtcNow
            };
        }
    }
}


