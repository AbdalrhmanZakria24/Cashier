using Fixawy.DTOS.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fixawy.Controllers
{
    [Area("Employee")]
    [Route("[area]/[controller]")]
    [ApiController]
    [Authorize(Roles = $"{Rl.Tentant}, {Rl.BranchManager} ,{Rl.SuperAdmin},{Rl.Cashier}")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        protected IActionResult HandleError(AdminResponse result)
        {
            return BadRequest(new ErrorMessage
            {
                code = "Error",
                message = result.message
            });
        }
        protected IActionResult HandleSuccess(AdminResponse result)
        {
            return Ok(new SuccessMessage
            {
                code = "Done",
                message = result.message
            });
        }

        [HttpGet("Get/{UserId}")]
        [Authorize(Roles = $"{Rl.Tentant}, {Rl.BranchManager} ,{Rl.SuperAdmin},{Rl.Cashier}")]
        public async Task<IActionResult> Get([FromRoute] string UserId, [FromQuery] string? Code, CancellationToken cancellationToken)
        {
            var result = await _cartService.GetCart(UserId, Code, cancellationToken);

            if (!result.IsSuccess)
            {
                return BadRequest(new ErrorMessage
                {
                    code = "Error",
                    message = result.Massege
                });
            }

            return Ok(new
            {
                Status = new SuccessMessage
                {
                    code = "Done",
                    message = result.Massege
                },
                Data = new
                {
                    Data = result.data
                },
                OverView = new
                {
                    TotalProduct = result.data.TotalAmount,
                    TotalCount = result.data.ItemsCount
                }
            });

        }

        [HttpPost("AddInCart/{UserId}/{ProductId}")]
        [Authorize(Roles = $"{Rl.Tentant}, {Rl.BranchManager} ,{Rl.SuperAdmin},{Rl.Cashier}")]
        public async Task<IActionResult> AddInCart([FromRoute] string UserId, [FromRoute] long ProductId, [FromQuery] decimal Quantity, CancellationToken cancellationToken)
        {
            var result = await _cartService.AddInCart(ProductId, UserId, Quantity, cancellationToken);

            if (!result.isSuccess)
                return HandleError(result);

            return HandleSuccess(result);
        }

        [HttpPatch("InCrease/{UserId}/{ProductId}")]
        [Authorize(Roles = $"{Rl.Tentant}, {Rl.BranchManager} ,{Rl.SuperAdmin},{Rl.Cashier}")]
        public async Task<IActionResult> InCrease([FromRoute] string UserId, [FromRoute] long ProductId, CancellationToken cancellationToken)
        {
            var result = await _cartService.Increase(ProductId, UserId, cancellationToken);

            if (!result.isSuccess)
                return HandleError(result);

            return HandleSuccess(result);
        }

        [HttpPatch("DeCrease/{UserId}/{ProductId}")]
        [Authorize(Roles = $"{Rl.Tentant}, {Rl.BranchManager} ,{Rl.SuperAdmin},{Rl.Cashier}")]
        public async Task<IActionResult> Decrease([FromRoute] string UserId, [FromRoute] long ProductId, CancellationToken cancellationToken)
        {
            var result = await _cartService.Decrease(ProductId, UserId, cancellationToken);

            if (!result.isSuccess)
                return HandleError(result);

            return HandleSuccess(result);
        }

        [HttpDelete("Remove/{UserId}/{ProductId}")]
        [Authorize(Roles = $"{Rl.Tentant}, {Rl.BranchManager} ,{Rl.SuperAdmin},{Rl.Cashier}")]
        public async Task<IActionResult> Remove([FromRoute] string UserId, [FromRoute] long ProductId, CancellationToken cancellationToken)
        {
            var result = await _cartService.Remove(ProductId, UserId, cancellationToken);

            if (!result.isSuccess)
                return HandleError(result);

            return HandleSuccess(result);
        }
    }
}
