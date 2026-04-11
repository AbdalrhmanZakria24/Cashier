using System.Security.Claims;

namespace Fixawy.Areas.Identity.Services.Interface
{
    public interface ITokenService
    {
        public Task<string> GenerateJwtTokenAsync(ApplicationUser user);
        public string GenerateRefreshToken();
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
