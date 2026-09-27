using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace Cashier.Application.CQRS.Command.Identity.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, ResultT<LoginResponse>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IRefreshTokenGenerator _refreshToken;
        private readonly IConfiguration _configuration;

        public LoginCommandHandler(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IUnitOfWork unitOfWork,
            IJwtTokenService jwtTokenService,
            IRefreshTokenGenerator refreshToken,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _jwtTokenService = jwtTokenService;
            _refreshToken = refreshToken;
            _configuration = configuration;
        }
        public async Task<ResultT<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
                return ResultT<LoginResponse>.Failure(new Error("Login.notFound",
                    "Invalid email", ErrorType.NotFound));

            if(!user.EmailConfirmed)
                return ResultT<LoginResponse>.Failure(new Error("Login.notFound",
                    "Email not confirmed ,confirm email first", ErrorType.Forbidden));

            var result = await _signInManager
                .PasswordSignInAsync(user, request.Password, request.RememberMe, true);

            if(result.IsLockedOut)
                return ResultT<LoginResponse>.Failure(new Error("Login.locked",
                    "Account locked. Try again later", ErrorType.Forbidden));

            if(!result.Succeeded)
                return ResultT<LoginResponse>.Failure(new Error("Login.invalidValidation",
                    "Invalid email or password", ErrorType.Forbidden));

            var AccessToken = await _jwtTokenService.GenerateJwtTokenAsync(user);
            var RefreshToken = _refreshToken.GenerateRefreshToken();
            var RefreshTokenExpireTime = DateTime.UtcNow.AddDays(7);

            var refreshToken = new Cashier.Domain.Entities.RefreshToken(user.Id, RefreshToken, RefreshTokenExpireTime);

            await _unitOfWork.RefreshTokenreposatory.CreateAsync(refreshToken,cancellationToken);

            await _unitOfWork.CommitAsync(cancellationToken);

            var durationInMinutes = int.Parse(_configuration["Jwt:DurationInMinutes"]!);

            var finalResponse = new LoginResponse
            {
                AccessToken = AccessToken,
                RefreshToken = RefreshToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(durationInMinutes),
                RefreshTokenExpiryTime = RefreshTokenExpireTime
            };

            return ResultT<LoginResponse>.Success(finalResponse);
        }
    }
}
