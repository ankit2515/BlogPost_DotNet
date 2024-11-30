using BlogPost.web.Data;
using BlogPost.web.Models;
using Microsoft.AspNetCore.Identity;

namespace BlogPost.web.Utilities
{
    public class DbInitializer : IDbInitializer
    {
        private readonly BlogDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public DbInitializer(BlogDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public void Initialize()
        {
            if (!_roleManager.RoleExistsAsync(WebsiteRoles.WebsiteAdmin).GetAwaiter().GetResult())
            {  // will check if there is no admin
                _roleManager.CreateAsync(new IdentityRole(WebsiteRoles.WebsiteAdmin)).GetAwaiter().GetResult(); //create admin
                _roleManager.CreateAsync(new IdentityRole(WebsiteRoles.WebsiteAuthor)).GetAwaiter().GetResult(); // create author
                //create user with username and passwords
                _userManager.CreateAsync(new ApplicationUser()
                {

                    UserName = "admin@bhadwa.com",
                    Email = "admin@gandwa.com",
                    FirstName = "Super",
                    LastName = "Admin"
                }, "Admin@1234").Wait();

                var appUser = _context.applicationUsers.FirstOrDefault(x => x.Email == "admin@gandwa.com"); //check if user has created or not

                if (appUser != null)
                {
                    _userManager.AddToRoleAsync(appUser, WebsiteRoles.WebsiteAdmin).GetAwaiter().GetResult();
                }

                var listOfPPage = new List<Page>()
                {
                    new Page()
                    {
                        Title = "About-us",
                         Slug= "about"
                    },
                    new Page()
                    {
                        Title = "Contact-us",
                        Slug = "contact"
                    },
                    new Page()
                    {
                        Title = "Privacy-Policy",
                        Slug = "privacy"
                    }
                };
                _context.pages.AddRange(listOfPPage);
                _context.SaveChanges();
            }
        }
    }
}
