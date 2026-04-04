using Microsoft.AspNetCore.Identity;

namespace Fixawy.Areas.Identity.Model
{
    public class ApplicationUser :IdentityUser
    {
        public string Address { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public string? JobTitle { get; set; } 
        public string? Description { get; set; } 

        public bool Isvalid { get; set; }
    }
}
