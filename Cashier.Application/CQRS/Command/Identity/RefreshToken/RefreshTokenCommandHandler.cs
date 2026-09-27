using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

namespace Cashier.Application.CQRS.Command.Identity.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, ResultT<LoginResponse>>
    {
        private readonly IRefreshTokenGenerator _refreshToken;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public RefreshTokenCommandHandler(IRefreshTokenGenerator refreshToken,
            IJwtTokenService jwtTokenService,
            UserManager<ApplicationUser> userManager,
            IUnitOfWork unitOfWork,
            IConfiguration configuration)
        {
            _refreshToken = refreshToken;
            _jwtTokenService = jwtTokenService;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<ResultT<LoginResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            ClaimsPrincipal principal;

            try
            {
                principal = _refreshToken.GetPrincipalFromExpiredToken(request.AccessToken);
            }
            catch
            {
                return ResultT<LoginResponse>.Failure(new Error(
                    "Identity.RefreshToken",
                    "Invalid access token",
                    ErrorType.Failure));
            }
            var userId = principal.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (userId is null)
                return ResultT<LoginResponse>.Failure(new Error("Identity.RefreshToken", "Can not found user id",
                    ErrorType.NotFound));

            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
                return ResultT<LoginResponse>.Failure(new Error("Identity.RefreshToken",
                    "Can not found user", ErrorType.NotFound));

            if (!user.IsActive)
            {
                return ResultT<LoginResponse>.Failure(
                    new Error(
                        "Identity.RefreshToken",
                        "User account is inactive.",
                        ErrorType.Failure));
            }

            var refresh = await _unitOfWork.RefreshTokenreposatory
              .GetOneAsync(x => x.Token == request.RefreshToken && x.ApplicationUserId == user.Id);

            if (refresh is null)
                return ResultT<LoginResponse>.Failure(new Error("Identity.RefreshToken",
                    "Can not found refresh token", ErrorType.NotFound));

            if (refresh.IsRevoked || refresh.ExpiresOnUtc <= DateTime.UtcNow)
                return ResultT<LoginResponse>.Failure(new Error("Identity.RefreshToken",
                    "Refresh token is not valid", ErrorType.Failure));

            refresh.Revoke();

            _unitOfWork.RefreshTokenreposatory.Update(refresh);


            var accessToken = await _jwtTokenService.GenerateJwtTokenAsync(user);
            var refreshToken = _refreshToken.GenerateRefreshToken();

            var expires = DateTime.UtcNow.AddDays(7);

            var refreshTokenEntity = new Cashier.Domain.Entities.RefreshToken(user.Id, refreshToken, expires);

            await _unitOfWork.RefreshTokenreposatory.CreateAsync(refreshTokenEntity, cancellationToken);

            await _unitOfWork.CommitAsync(cancellationToken);

            var durationInMinutes = int.Parse(_configuration["Jwt:DurationInMinutes"]!);


            var FinalResponse = new LoginResponse
            {
                UserId = user.Id,
                AccessToken = accessToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(durationInMinutes),
                RefreshToken = refreshToken,
                RefreshTokenExpiryTime = expires
            };

            return ResultT<LoginResponse>.Success(FinalResponse);
        }
    }
}
