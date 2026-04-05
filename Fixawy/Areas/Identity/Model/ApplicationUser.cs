using Microsoft.AspNetCore.Identity;

namespace Fixawy.Areas.Identity.Model
{
    public class ApplicationUser :IdentityUser
    {
        public string Address { get; set; } = string.Empty;
        public string fullName { get; set; } = string.Empty;
       
    }
}
