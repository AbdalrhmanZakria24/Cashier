using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;

namespace Fixawy.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("[area]/[controller]")]
    [ApiController]
    [Authorize(Roles = $"{Rl.Tentant}, {Rl.BranchManager} ,{Rl.SuperAdmin},{Rl.Cashier}")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public IActionResult HandleError(AdminResponse result)
        {
            return BadRequest(new ErrorMessage
            {
                code = "Error",
                message = result.message,
                createdAt = DateTime.UtcNow
            });
        }
        public IActionResult HandleSuccess(AdminResponse result)
        {
            return Ok(new SuccessMessage
            {
                code = "Success",
                message = result.message,
                createdAt = DateTime.UtcNow
            });
        }
        [HttpGet(nameof(Get))]
        [Authorize(Roles = $"{Rl.Tentant}, {Rl.BranchManager} ,{Rl.SuperAdmin},{Rl.Cashier}")]
        public async Task<IActionResult> Get([FromQuery]ProductSearch? productSearch, [FromQuery]Pagination pagination)
        {
            var result = await _productService.Get(productSearch, pagination);

            if (!result.isSuccess)
                return HandleError(result);

            return Ok(new
            {
                Status = new
                {
                    code = "Success",
                    message = result.message,
                    createdAt = DateTime.UtcNow
                },
                Data = new
                {
                    CurrentNumber = result.currentPage,
                    TotalPage = result.totalPages,
                    TotalCount = result.totalCount,
                    Data = result.Data
                }

            });
        }

        [HttpPost(nameof(Create))]
        [Authorize(Roles = $"{Rl.Tentant}, {Rl.BranchManager} ,{Rl.SuperAdmin},{Rl.Cashier}")]
        public async Task<IActionResult> Create([FromBody]AddProduct addProduct, CancellationToken cancellationToken)
        {
            var result = await _productService.Create(addProduct, cancellationToken);

            if (!result.isSuccess)
                return HandleError(result);

            return HandleSuccess(result);
        }

        [HttpPut("Update/{id}")]
        [Authorize(Roles = $"{Rl.Tentant}, {Rl.BranchManager} ,{Rl.SuperAdmin},{Rl.Cashier}")]
        public async Task<IActionResult> Update ([FromBody]UpdateProduct update ,[FromRoute] long id , CancellationToken cancellationToken)
        {
            var result =await _productService.Update(update , id , cancellationToken);

            if (!result.isSuccess)
                return HandleError(result);

            return HandleSuccess(result);
        }

        [HttpDelete("Delete/{id}/{branchId}")]
        [Authorize(Roles = $"{Rl.Tentant}, {Rl.BranchManager} ,{Rl.SuperAdmin},{Rl.Cashier}")]
        public async Task<IActionResult> Delete( [FromRoute]long id , [FromRoute]long branchId)
        {
            var result =await _productService.Delete( id , branchId);

            if (!result.isSuccess)
                return HandleError(result);

            return HandleSuccess(result);
        }
    }
}
