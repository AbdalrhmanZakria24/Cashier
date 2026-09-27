

namespace Cashier.Application.Interface
{
    public interface IJwtTokenService
    {
        public Task<string> GenerateJwtTokenAsync(ApplicationUser user);
    }
}

