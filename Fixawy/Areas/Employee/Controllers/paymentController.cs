using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.Security.Claims;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Fixawy.Areas.Employee.Controllers
{
    [Area("Employee")]
    [Route("[Area]/[controller]")]
    [ApiController]
    [Authorize]
    public class paymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IUnitOfWork _unitOfWork;

        public paymentController(
            IPaymentService paymentService,
            IUnitOfWork unitOfWork)
        {
            _paymentService = paymentService;
            _unitOfWork = unitOfWork;
        }

        [HttpPost("pay")]
        public async Task<IActionResult> Pay(
            CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await _paymentService.CreateStripePaymentIntent(userId, cancellationToken);

            if (!result.isSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("success")]
        public async Task<IActionResult> Success(string session_id, CancellationToken cancellationToken)
        {
            var payment = await _unitOfWork.Paymentreposatory
                .GetOneAsync(
           p => p.TransactionId == session_id,
           cancellationToken: cancellationToken);

            if (payment == null)
                return NotFound();
            var invoice = await _unitOfWork.InvoiceReposatory
                .GetOneAsync(
           i => i.Order.Payment!.Id == payment.Id,
           new Expression<Func<Invoice, object>>[]
           {
                i => i.Order,
                i => i.Order.OrderItems,
                i => i.Order.OrderItems.Select(x => x.Product)
           },
           cancellationToken: cancellationToken);

            if (invoice == null)
                return NotFound();

            return await GenerateInvoicePdf(invoice);
        }

        private async Task<IActionResult> GenerateInvoicePdf(Invoice invoice)
        {
            var pdf = QuestPDF.Fluent.Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(20);

                    page.Header()
                        .Text($"Invoice #{invoice.InvoiceNumber}")
                        .FontSize(22);

                    page.Content().Column(col =>
                    {
                        col.Item().Text($"Date: {invoice.IssuedAt}");

                        col.Item().Text($"Total: {invoice.Total} EGP");

                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(4);
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("Product");
                                header.Cell().Text("Qty");
                                header.Cell().Text("Price");
                                header.Cell().Text("Total");
                            });

                            foreach (var item in invoice.Order.OrderItems)
                            {
                                table.Cell().Text(item.Product.Name);
                                table.Cell().Text(item.Quantity.ToString());
                                table.Cell().Text(item.UnitPrice.ToString());
                                table.Cell().Text(item.Total.ToString());
                            }
                        });
                    });
                });
            });

            var bytes = pdf.GeneratePdf();

            return File(
                bytes,
                "application/pdf",
                $"Invoice-{invoice.InvoiceNumber}.pdf");
        }
    }
}
