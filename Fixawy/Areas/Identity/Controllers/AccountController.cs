using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fixawy.Areas.Identity.Controllers
{
    [Area("Identity")]
    [Route("[area]/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Register register, CancellationToken cancellationToken)
        {
            var result = await _accountService.Register(register, Request.Scheme);

            if(!result.isSuccess)
            {
                return BadRequest(new ErrorMessage
                {
                    code="Error",
                    message=result.message,
                    createdAt=DateTime.Now  
                });
            }

            return Ok(new SuccessMessage
            {
                code = "success",
                message=result.message,
                createdAt = DateTime.Now
            });
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail([FromQuery]string token, [FromQuery] string id)
        {
            var result=await _accountService.ConfirmEmail(token, id);

            if(!result.isSuccess)
            {
                return BadRequest(new ErrorMessage
                {
                    code="Error",
                    message=result.message,
                    createdAt =DateTime.UtcNow
                });
            }

            return Ok(new SuccessMessage
            {
                code = "success",
                message = result.message,
                createdAt = DateTime.UtcNow
            });
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody]Login login)
        {
            var result=await _accountService.Login(login);

            if (!result.isSuccess)
            {
                return BadRequest(new ErrorMessage
                {
                    code = "Error",
                    message = result.message,
                    createdAt = DateTime.UtcNow
                });
            }

            return Ok(new SuccessMessage
            {
                code="success",
                message= result.message,
                createdAt= DateTime.UtcNow,
                AccessToken=result.AccessToken,
                ExpiresAt=result.ExpiresAt,
                RefreshToken=result.RefreshToken,
                RefreshTokenExpiryTime=result.RefreshTokenExpiryTime,
            });
        }

        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword([FromBody]ForgetPassword forgetPassword,CancellationToken cancellationToken)
        {
            var result=await _accountService.ForgetPassword(forgetPassword,cancellationToken);

            if (!result.isSuccess)
            {
                return BadRequest(new ErrorMessage
                {
                    code = "Error",
                    message = result.message,
                    createdAt = DateTime.UtcNow
                });
            }

            return Ok(new SuccessMessage
            {
                code = "Success",
                message = result.message,
                createdAt = DateTime.UtcNow
            });
        }

        [HttpPost("ValidateOtp")]

        public async Task<IActionResult> ValidateOtp([FromBody] ValidateOtp validateOtp,[FromQuery]string id)
        {
            var result = await _accountService.ValidateOtp(validateOtp, id);

            if (!result.isSuccess)
            {
                return BadRequest(new ErrorMessage
                {
                   code= "Error",
                   message= result.message,
                   createdAt= DateTime.UtcNow
                });
            }

            return Ok(new SuccessMessage
            {
                code = "success",
                message = result.message,
                createdAt = DateTime.UtcNow,
                ResetToken = result.ResetToken
            });
        }
        [HttpPost("ResetPassword")]

        public async Task<IActionResult> ResetPassword([FromBody] ResetPassword resetPassword,[FromQuery]string ResetToken, [FromQuery]string id)
        {
            var result = await _accountService.ResetPassword(resetPassword,ResetToken, id);

            if (!result.isSuccess)
            {
                return BadRequest(new ErrorMessage
                {
                   code= "Error",
                   message= result.message,
                   createdAt= DateTime.UtcNow
                });
            }

            return Ok(new SuccessMessage
            {
                code = "success",
                message = result.message,
                createdAt = DateTime.UtcNow,
            });
        }

    }
}
