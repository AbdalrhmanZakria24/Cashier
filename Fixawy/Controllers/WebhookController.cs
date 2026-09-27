using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using System.Linq.Expressions;

namespace Fixawy.Controllers
{
    [Area("Employee")]
    [Route("[Area]/[controller]")]
    [ApiController]
    public class WebhookController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public WebhookController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("stripe-webhook")]
        public async Task<IActionResult> StripeWebhook(CancellationToken cancellationToken)
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            var stripeEvent = EventUtility.ConstructEvent(
                json,
                Request.Headers["Stripe-Signature"],
                "WEBHOOK_SECRET"
            );

            if (stripeEvent.Type == "checkout.session.completed")
            {
                var session = stripeEvent.Data.Object as Session;

                var payment = await _unitOfWork.Paymentreposatory
                     .GetOneAsync(p => p.TransactionId == session!.Id,
                     new Expression<Func<Payment, object>>[]
                     {
                         p => p.cart,
                         p => p.cart.CartItems,
                         p => p.cart.CartItems.Select(c => c.Product)
                     });

                if (payment == null)
                    return BadRequest();

                if (payment!.Status == PaymentStatus.Paid)
                {
                    return Ok();
                }

                payment!.Status = PaymentStatus.Paid;
                payment.PaidAt = DateTime.UtcNow;

                var TenantId = payment.TenantId;

                var order = new Order
                {
                    TenantId = TenantId,
                    UserId = payment.ApplicationUserId,
                    Total = payment.Amount,
                    Status = OrderStatus.Paid,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _unitOfWork.Orderreposatory.CreateAsync(order,cancellationToken);
                await _unitOfWork.CommitAsync();

               
                foreach (var item in payment.cart.CartItems)
                {
                    var orderItem = new OrderItem
                    {
                        TenantId = TenantId,
                        OrderId = order.Id,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        Total = item.Total,
                        Discount = item.Discount,
                        Tax = 0
                    };

                    await _unitOfWork.OrderItemReposatory.CreateAsync(
                        orderItem,
                        cancellationToken);
                }

                await _unitOfWork.CommitAsync();

                var invoice = new Model.Invoice
                {
                    TenantId = order.TenantId,
                    OrderId = order.Id,
                    InvoiceNumber = $"INV-{order.Id}",
                    SubTotal = order.SubTotal,
                    Discount = order.Discount,
                    Tax = order.Tax,
                    Total = order.Total,
                    IsPaid = true,
                    IssuedAt = DateTime.UtcNow
                };

                await _unitOfWork.InvoiceReposatory
                    .CreateAsync(invoice, cancellationToken);

                await _unitOfWork.CommitAsync();


                _unitOfWork.RemoveRangeCartItemReposatories.RemoveRange(payment.cart.CartItems);

                await _unitOfWork.CommitAsync();
            }

            return Ok();
        }
    }
}
