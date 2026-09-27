using System;
using System.Collections.Generic;
using System.Text;

namespace Cashier.Domain.Entities
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; } = string.Empty;
        public ApplicationUser ApplicationUser { get; set; } = default!;
        public string Token { get; private set; } = string.Empty;
        public DateTimeOffset ExpiresOnUtc { get; private set; }
        public bool IsRevoked { get; private set; }

        protected RefreshToken()
        {
        }

        public RefreshToken(string userId, string token, DateTimeOffset expiresOnUtc)
        {
            ApplicationUserId = userId;
            Token = token;
            ExpiresOnUtc = expiresOnUtc;
        }

        public void Revoke() => IsRevoked = true;
        public void Restore() => IsRevoked = false;
    }
}
