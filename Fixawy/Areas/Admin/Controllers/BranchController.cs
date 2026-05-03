using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fixawy.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    [Route("[area]/[controller]")]
    [ApiController]
    public class BranchController : ControllerBase
    {
        private readonly IBranchServices _branchServices;

        public BranchController(IBranchServices branchServices)
        {
            _branchServices = branchServices;
        }

        protected IActionResult HandelError(AdminResponse result)
        {
            return BadRequest(new ErrorMessage
            {
                code = "Error",
                message = result.message,
                createdAt = DateTime.UtcNow
            });
        }

        protected IActionResult HandelSuccess(AdminResponse result)
        {
            return Ok(new SuccessMessage
            {
                code = "success",
                message = result.message,
                createdAt = DateTime.UtcNow
            });
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get([FromQuery] BranchSerch? branchSerch, [FromQuery] Pagination pagination)
        {
            var result = await _branchServices.Get(branchSerch, pagination);

            if (!result.isSuccess)
            {
                return HandelError(result);
            }
            return Ok(new
            {
                code = "success",
                message = result.message,
                createdAt = DateTime.UtcNow,
                meta = new
                {
                    totalBranches = result.totalCount,
                    totalPages = result.totalPages,
                    pageSize = pagination.PageSize,
                    data = result.Data
                }
            });
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] AddBranch addBranch, CancellationToken cancellationToken)
        {
            var result = await _branchServices.Create(addBranch, cancellationToken);
            if (!result.isSuccess)
            {
                return HandelError(result);
            }

            return HandelSuccess(result);
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update([FromBody] DTOS.Request.Branch.UpdateBranch UpdateBranch,[FromRoute] long id)
        {
            var result = await _branchServices.Update(UpdateBranch, id);

            if(!result.isSuccess)
                return HandelError(result);

            return HandelSuccess(result);
        }
    }
}
