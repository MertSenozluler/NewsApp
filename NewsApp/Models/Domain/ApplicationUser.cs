using Microsoft.AspNetCore.Identity;

namespace NewsApp.Models.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string? ProfilePicture { get; set; }
        public ICollection<Comments> Comments { get; set; }
    }
}
