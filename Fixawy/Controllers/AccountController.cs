namespace Fixawy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(
                  RegisterCommand command)
        {
            var result =
                await _mediator.Send(command);

            return this.ToActionResult<string>(result);
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(
             [FromQuery] string id,
             [FromQuery] string token)
        {
            var result =
                await _mediator.Send(
                    new ConfirmEmailCommand(id, token));

            return this.ToActionResult<bool>(result);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginCommand command)
        {
            var result =
               await _mediator.Send(command);

            return this.ToActionResult<LoginResponse>(result);
        }

        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordCommand forgetPassword)
        {
            var result =
             await _mediator.Send(forgetPassword);

            return this.ToActionResult<bool>(result);
        }

        [HttpPost("ValidateOtp")]

        public async Task<IActionResult> ValidateOtp([FromBody]string Otp, [FromQuery]string id)
        {
            var result =
            await _mediator.Send(new ValidateOtpCommand(id,Otp));

            return this.ToActionResult<string>(result);
        }
        [HttpPost("ResetPassword")]

        public async Task<IActionResult> ResetPassword([FromBody] string Password,
            [FromQuery]string ResetToken, [FromQuery]string id)
        {
            var result =
             await _mediator.Send(new ResetPasswordCommand(Password,ResetToken, id));

            return this.ToActionResult<string>(result);
        }

    }
}
