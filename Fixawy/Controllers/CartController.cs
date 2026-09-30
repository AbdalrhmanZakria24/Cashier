
namespace Fixawy.Areas.Employee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = $"{Rl.BranchManager},{Rl.SuperAdmin},{Rl.Cashier}")]
    public class CartController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CartController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetCart(
            string? code,
            CancellationToken cancellationToken)
        {
            var userId =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _mediator.Send(
                new GetCartQuery(
                    userId!,
                    code),
                cancellationToken);

            return this.ToActionResult<CartResponseDto>(result);
        }

        [HttpPost("items")]
        public async Task<IActionResult> AddToCart(
            AddToCartCommand request,
            CancellationToken cancellationToken)
        {
            var userId =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _mediator.Send(
                new AddToCartCommand(
                    request.ProductId,
                    userId!,
                    request.Quantity),
                cancellationToken);

            return this.ToActionResult<bool>(result);
        }

        [HttpPost("items/{productId}/increase")]
        public async Task<IActionResult> Increase(
            long productId,
            CancellationToken cancellationToken)
        {
            var userId =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _mediator.Send(
                new IncreaseCartItemCommand(
                    productId,
                    userId!),
                cancellationToken);

            return this.ToActionResult<bool>(result);
        }

        [HttpPost("items/{productId}/decrease")]
        public async Task<IActionResult> Decrease(
            long productId,
            CancellationToken cancellationToken)
        {
            var userId =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _mediator.Send(
                new DecreaseCartItemCommand(
                    productId,
                    userId!),
                cancellationToken);

            return this.ToActionResult<bool>(result);
        }

        [HttpDelete("items/{productId}")]
        public async Task<IActionResult> Remove(
            long productId,
            CancellationToken cancellationToken)
        {
            var userId =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _mediator.Send(
                new RemoveFromCartCommand(
                    productId,
                    userId!),
                cancellationToken);

            return this.ToActionResult<bool>(result);
        }
    }
}