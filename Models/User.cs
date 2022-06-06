using Microsoft.AspNetCore.Identity;

namespace DisneyAPI.Models
{
    public class User : IdentityUser
    {
        public bool IsActive { get; set; } = true;
    }
}
