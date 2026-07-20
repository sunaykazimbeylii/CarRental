using Microsoft.AspNetCore.Identity;

namespace CarRental.Domain.Entities
{
    public class AppUser:IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}
