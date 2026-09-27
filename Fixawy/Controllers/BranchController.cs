namespace Fixawy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = $"{Rl.BranchManager},{Rl.SuperAdmin},{Rl.Cashier}")]
    public class BranchController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BranchController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(nameof(Get))]
        [Authorize(Roles =
            $"{Rl.Tentant},{Rl.BranchManager},{Rl.SuperAdmin},{Rl.Cashier}")]
        public async Task<IActionResult> Get(
            [FromQuery] BranchSerch branchSerch,
            [FromQuery] Pagination pagination,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new GetBranchesQuery(branchSerch,pagination));

            return this.ToActionResult<BranshResponse>(result);
        }

        [HttpPost(nameof(Create))]
        [Authorize(Roles =
            $"{Rl.Tentant},{Rl.BranchManager},{Rl.SuperAdmin},{Rl.Cashier}")]
        public async Task<IActionResult> Create(
            [FromBody] CreateBranchCommand command,
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
            [FromBody] UpdateBranchCommand command,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                command with { Id = id },
                cancellationToken);

            return this.ToActionResult<long>(result);
        }
    }
}