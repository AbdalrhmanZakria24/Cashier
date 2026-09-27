using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Cashier.Application.Interface
{
    public interface IRefreshTokenGenerator
    {
        public string GenerateRefreshToken();
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
