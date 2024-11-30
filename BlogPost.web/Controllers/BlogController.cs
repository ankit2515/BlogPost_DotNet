using AspNetCoreHero.ToastNotification.Abstractions;
using BlogPost.web.Data;
using BlogPost.web.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace BlogPost.web.Controllers
{
    public class BlogController : Controller
    {
        private readonly BlogDbContext dbContext;
        private readonly INotyfService notyfService;
        public BlogController(BlogDbContext dbContext,  INotyfService notyfService)
        {
            this.dbContext = dbContext;
            this.notyfService = notyfService;
        }

        [HttpGet("[controller]/{slug}")]
        public IActionResult Post(string slug)
        {
            if (slug == "")
            {
                notyfService.Information("Post Not Found!");
                return View();
            }

            var post = dbContext.posts!.Include(x=>x.ApplicationUser).FirstOrDefault(x=> x.slug==slug);

            if (post == null) {
                notyfService.Information("Post Not Found!");
                return View();
            }

            var vm = new BlogpostVM()
            {
                Id = post.Id,
                Title = post.Title,
                AuthorName=post.ApplicationUser!.FirstName + " " + post.ApplicationUser!.LastName,
                ThumbnailUrl = post.ThumbnailUrl,
                CreatedDate = post.CreatedDate,
                ShortDesscription=post.ShortDescription,
                Description = post.Description,
            };

            return View(vm);
        }
    }
}
