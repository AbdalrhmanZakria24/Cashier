
namespace Fixawy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = $"{Rl.BranchManager},{Rl.SuperAdmin},{Rl.Cashier}")]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(nameof(Get))]
        [Authorize(Roles = $"{Rl.Tentant},{Rl.BranchManager},{Rl.SuperAdmin},{Rl.Cashier}")]
        public async Task<IActionResult> Get(
            [FromQuery] ProductSearch? productSearch,
            [FromQuery] Pagination pagination,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new GetProductsQuery(productSearch, pagination));

            return this.ToActionResult<ProductResponse>(result);
        }

        [HttpPost(nameof(Create))]
        [Authorize(Roles = $"{Rl.BranchManager},{Rl.SuperAdmin},{Rl.Cashier}")]
        public async Task<IActionResult> Create(
            [FromBody] CreateProductCommand command,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                command,
                cancellationToken);

            return this.ToActionResult<long>(result);
        }

        [HttpPut("Update/{id:long}")]
        [Authorize(Roles = $"{Rl.BranchManager},{Rl.SuperAdmin},{Rl.Cashier}")]
        public async Task<IActionResult> Update(
            [FromRoute] long id,
            [FromBody] UpdateProductCommand command,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                command with
                {
                    Id = id
                },
                cancellationToken);

            return this.ToActionResult<long>(result);
        }

        [HttpDelete("Delete/{id:long}/{branchId:long}")]
        [Authorize(Roles = $"{Rl.Tentant},{Rl.BranchManager},{Rl.SuperAdmin},{Rl.Cashier}")]
        public async Task<IActionResult> Delete(
            [FromRoute] long id,
            [FromRoute] long branchId,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new DeleteProductCommand(
                    id,
                    branchId),
                cancellationToken);

            return this.ToActionResult<long>(result);
        }
    }
}
