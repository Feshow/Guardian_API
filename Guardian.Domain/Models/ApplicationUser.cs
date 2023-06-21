using Microsoft.AspNetCore.Identity;

namespace Guardian.Domain.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
