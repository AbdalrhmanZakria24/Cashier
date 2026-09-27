using System;
using System.Collections.Generic;
using System.Text;

namespace Cashier.Application.DTO
{
    public class LoginResponse
    {
        public string UserId { get; set; } = string.Empty;
        public string AccessToken { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
