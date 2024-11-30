using BlogPost.web.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace BlogPost.web.Data
{
    public class BlogDbContext: IdentityDbContext
    {
        public BlogDbContext(DbContextOptions<BlogDbContext> option) : base(option)
        {

        }
        public DbSet<ApplicationUser> applicationUsers { get; set; }
        public DbSet<Post>? posts { get; set; }
        public DbSet<Page>? pages { get; set; }
        public DbSet<Setting>? settings { get; set; }
    }
}
