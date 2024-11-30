using BlogPost.web.Data;
using BlogPost.web.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogPost.web.Controllers
{
    public class PageController : Controller
    {
        private readonly BlogDbContext _dbContext;
        public PageController(BlogDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }
        public async Task<IActionResult> About()
        {
            var page = await _dbContext.pages!.FirstOrDefaultAsync(x => x.Slug == "about");
            var vm = new PageVM()
            {
                Id = page!.Id,
                Title = page.Title,
                ShortDescription = page.ShortDescription,
                Description = page.Description,
                ThumbnailUrl = page.ThumbnailUrl,
            };

            return View(vm);
        }
        public async Task<IActionResult> Contact()
        {
            var page = await _dbContext.pages!.FirstOrDefaultAsync(x => x.Slug == "contact");
            var vm = new PageVM()
            {
                Id = page!.Id,
                Title = page.Title,
                ShortDescription = page.ShortDescription,
                Description = page.Description,
                ThumbnailUrl = page.ThumbnailUrl,
            };

            return View(vm);
        }
        public async Task<IActionResult> PrivacyPolicy()
        {
            var page = await _dbContext.pages!.FirstOrDefaultAsync(x => x.Slug == "privacy");
            var vm = new PageVM()
            {
                Id = page!.Id,
                Title = page.Title,
                ShortDescription = page.ShortDescription,
                Description = page.Description,
                ThumbnailUrl = page.ThumbnailUrl,
            };

            return View(vm);
        }
    }
}
