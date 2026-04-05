using System.ComponentModel.DataAnnotations;

namespace Fixawy.Areas.Identity.DTOS.Request
{
    public class RegisterWorker : Register
    {
        [Required(ErrorMessage ="Please enter your location")]
        public string location { get; set; } =string.Empty;
        [Required(ErrorMessage = "Please enter your national id image")]
        public IFormFile nationalIdImage { get; set; }

        public int serviceId { get; set; }
    }
}
