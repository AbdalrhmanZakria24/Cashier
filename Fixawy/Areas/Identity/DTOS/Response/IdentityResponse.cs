namespace Fixawy.Areas.Identity.DTOS.Response
{
    public class IdentityResponse
    {
        public Guid id { get; set; }= Guid.NewGuid();
        public bool isSuccess { get; set; }
        public string message { get; set; } = string.Empty;
        public DateTime createdAt { get; set; }
    }
}
