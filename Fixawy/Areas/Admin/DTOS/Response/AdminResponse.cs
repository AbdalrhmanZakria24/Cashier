namespace Fixawy.Areas.Admin.DTOS.Response
{
    public class AdminResponse
    {
        public Guid id { get; set; } = Guid.NewGuid();
        public bool isSuccess { get; set; }
        public string message { get; set; }=string.Empty;
        public DateTime createAt { get; set; }
    }
}
