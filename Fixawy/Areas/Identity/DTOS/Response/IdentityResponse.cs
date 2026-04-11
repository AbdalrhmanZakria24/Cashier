namespace Fixawy.Areas.Identity.DTOS.Response
{
    public class IdentityResponse
    {
        public Guid id { get; set; }= Guid.NewGuid();
        public bool isSuccess { get; set; }
        public string message { get; set; } = string.Empty;
        public DateTime createdAt { get; set; }

        public string? AccessToken { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public string? ResetToken { get; set; }

    }
}
