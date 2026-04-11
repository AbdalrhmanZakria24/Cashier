namespace Fixawy.ResponseStatus
{
    public class SuccessMessage
    {
        public Guid id = Guid.NewGuid();

        public string message { get; set; } = string.Empty;

        public string code { get; set; } = string.Empty;

        public DateTime createdAt { get; set; }
        public string? AccessToken { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public string? ResetToken { get; set; }

    }
}
