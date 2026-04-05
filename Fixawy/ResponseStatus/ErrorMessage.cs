namespace Fixawy.ResponseStatus
{
    public class ErrorMessage
    {
        public Guid id = Guid.NewGuid();

        public string message {  get; set; } = string.Empty;

        public string code { get; set; }= string.Empty;

        public DateTime createdAt { get; set; }
    }
}
