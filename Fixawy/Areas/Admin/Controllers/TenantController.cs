using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fixawy.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    [Route("[area]/[controller]")]
    [ApiController]
    public class TenantController : ControllerBase
    {
        private readonly ITenantService _tenantService;

        public TenantController(ITenantService tenantService)
        {
            _tenantService = tenantService;
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get([FromQuery]TenantSearch tenantSearch,[FromQuery]Pagination pagination)
        {
            var result=await _tenantService.GetAll(tenantSearch, pagination);

            if (!result.isSuccess)
            {
                return BadRequest(new ErrorMessage
                {
                    code="Error",
                    message=result.message,
                    createdAt=DateTime.UtcNow
                });
            }

            return Ok(new 
            {
                Success = new SuccessMessage
                {
                    code = "Done",
                    createdAt = DateTime.UtcNow,
                    message = result.message!,
                },
                Meta = new
                {
                    TotalTenant = result.totalCount,
                    TotalPages = result.totalPages,
                    PageSize = pagination.PageSize,
                    Data = result.Data
                }
            });
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody]AddTenant addTenant,CancellationToken cancellationToken) 
        {
            var result=await _tenantService.Create(addTenant, cancellationToken);

            if (!result.isSuccess)
            {
                return BadRequest(new ErrorMessage
                {
                    code="Error",
                    message=result.message,
                    createdAt=DateTime.UtcNow
                });
            }

            return Ok(new SuccessMessage
            {
                code = "success",
                message = result.message,
                createdAt = DateTime.UtcNow
            });
        }
        [HttpPut("{tenantId}")]
        public async Task<IActionResult> Update(int tenantId, [FromBody] UpdateTenant updateTenant) 
        {
            var result=await _tenantService.Update(updateTenant, tenantId);

            if (!result.isSuccess)
            {
                return BadRequest(new ErrorMessage
                {
                    code="Error",
                    message=result.message,
                    createdAt=DateTime.UtcNow
                });
            }

            return Ok(new SuccessMessage
            {
                code = "success",
                message = result.message,
                createdAt = DateTime.UtcNow
            });
        }
        [HttpDelete("{tenantId}")]
        public async Task<IActionResult> Delete(int tenantId) 
        {
            var result=await _tenantService.Delete(tenantId);

            if (!result.isSuccess)
            {
                return BadRequest(new ErrorMessage
                {
                    code="Error",
                    message=result.message,
                    createdAt=DateTime.UtcNow
                });
            }

            return Ok(new SuccessMessage
            {
                code = "success",
                message = result.message,
                createdAt = DateTime.UtcNow
            });
        }


    }
}
