using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fixawy.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = $"{Rl.Tentant}, {Rl.BranchManager} ,{Rl.SuperAdmin}")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        protected IActionResult HandleError(AdminResponse response)
        {
            return BadRequest(new ErrorMessage
            {
                code = "Error",
                message = response.message,
                createdAt = DateTime.UtcNow
            });
        }

        protected IActionResult HandleSuccess(AdminResponse response)
        {
            return Ok(new SuccessMessage
            {
                code = "success",
                message = response.message,
                createdAt = DateTime.UtcNow
            });
        }

        [HttpGet("Get")]
        [Authorize(Roles = $"{Rl.Tentant}, {Rl.BranchManager} ,{Rl.SuperAdmin}")]
        public async Task<IActionResult> Get([FromQuery] CategorySerch? categorySerch, [FromQuery] Pagination pagination)
        {
            var result = await _categoryService.Get(categorySerch, pagination);

            if (!result.isSuccess)
                return HandleError(result);

            return Ok(new
            {
                code = "success",
                message = result.message,
                createdAt = DateTime.UtcNow,

                Data = new
                {
                    totalCategory = result.totalCount,
                    totalPage = result.totalPages,
                    CurrentPage = result.currentPage,
                    PageSize = result.pageSize,
                    Data = result.Data
                }
            });

        }

        [HttpPost("Create")]
        [Authorize(Roles = $"{Rl.Tentant}, {Rl.BranchManager} ,{Rl.SuperAdmin}")]
        public async Task<IActionResult> Create([FromBody] AddCategory addCategory, CancellationToken cancellationToken)
        {
            var result = await _categoryService.Create(addCategory, cancellationToken);

            if (!result.isSuccess)
                return HandleError(result);

            return HandleSuccess(result);
        }

        [HttpPut("Update/{id}")]
        [Authorize(Roles = $"{Rl.Tentant}, {Rl.BranchManager} ,{Rl.SuperAdmin}")]
        public async Task<IActionResult> Update([FromQuery] UpdateCategory updateCategory, [FromRoute] long id)
        {
            var result = await _categoryService.Update(updateCategory, id);

            if (!result.isSuccess)
                return HandleError(result);

            return HandleSuccess(result);
        }

        [HttpDelete("Delete/{id}")]
        [Authorize(Roles = $"{Rl.Tentant}, {Rl.BranchManager} ,{Rl.SuperAdmin}")]
        public async Task<IActionResult> Delete([FromRoute] long id)
        {
            var result = await _categoryService.Delete(id);

            if (!result.isSuccess)
                return HandleError(result);

            return HandleSuccess(result);
        }

    }
}
