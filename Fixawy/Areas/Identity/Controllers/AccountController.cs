using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fixawy.Areas.Identity.Controllers
{
    [Area("Identity")]
    [Route("api/[area]/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        //[HttpPost]
        //public async Task<IActionResult> RegisterWorker([FromBody]RegisterWorker registerWorker,CancellationToken cancellationToken)
        //{
        //    var result = await _accountService.RegisterWorker(registerWorker, cancellationToken);

            
        //}
    }
}
