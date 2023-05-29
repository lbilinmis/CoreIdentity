using Microsoft.AspNetCore.Identity;

namespace CoreIdentity.WebUI.Entities
{
    public class AppUser : IdentityUser
    {
        public string City { get; set; }
    }
}
