
namespace Fixawy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = $"{Rl.BranchManager},{Rl.SuperAdmin},{Rl.Cashier}")]
    public class CategoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(nameof(Get))]
        [Authorize(Roles =
            $"{Rl.Tentant},{Rl.BranchManager},{Rl.SuperAdmin},{Rl.Cashier}")]
        public async Task<IActionResult> Get(
            [FromQuery] string? name,
            [FromQuery] Pagination pagination,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new GetCategoriesQuery(name,pagination),
                cancellationToken);

            return this.ToActionResult<CategoryResponse>(result);
        }

        [HttpPost(nameof(Create))]
        [Authorize(Roles =
            $"{Rl.Tentant},{Rl.BranchManager},{Rl.SuperAdmin},{Rl.Cashier}")]
        public async Task<IActionResult> Create(
            [FromBody] CreateCategoryCommand command,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                command,
                cancellationToken);

            return this.ToActionResult<long>(result);
        }

        [HttpPut("Update/{id:long}")]
        [Authorize(Roles =
            $"{Rl.Tentant},{Rl.BranchManager},{Rl.SuperAdmin},{Rl.Cashier}")]
        public async Task<IActionResult> Update(
            [FromRoute] long id,
            [FromBody] UpdateCategoryCommand command,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                command with { Id = id },
                cancellationToken);

            return this.ToActionResult<long>(result);
        }

        [HttpDelete("Delete/{id:long}")]
        [Authorize(Roles =
            $"{Rl.Tentant},{Rl.BranchManager},{Rl.SuperAdmin},{Rl.Cashier}")]
        public async Task<IActionResult> Delete(
            [FromRoute] long id,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new DeleteCategoryCommand(id),
                cancellationToken);

            return this.ToActionResult<long>(result);
        }
    }
}