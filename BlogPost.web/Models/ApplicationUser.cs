using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;

namespace BlogPost.web.Models
{
    public class ApplicationUser: IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        //relation (on user can have many post)
        List<Post>? Posts { get; set; }
    }
}
