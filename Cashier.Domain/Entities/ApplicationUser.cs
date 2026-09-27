using Microsoft.AspNetCore.Identity;

namespace Cashier.Domain.Entities
{
    public class ApplicationUser :IdentityUser
    {
        public string Address { get; set; } = string.Empty;
        public string fullName { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        protected ApplicationUser()
        {
        }
        public ApplicationUser(string name ,string email,string phoneNumber)
        {
            fullName = name;
            Email = email;
            PhoneNumber = phoneNumber;
        }

        public void Disable() => IsActive = false;
        public void Enable() => IsActive = true;

    }
}
