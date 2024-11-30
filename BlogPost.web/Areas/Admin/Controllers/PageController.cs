using AspNetCoreHero.ToastNotification.Abstractions;
using BlogPost.web.Data;
using BlogPost.web.Models;
using BlogPost.web.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace BlogPost.web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class PageController : Controller
    { 
        private readonly INotyfService _notification;
        private readonly BlogDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PageController(INotyfService _notification, BlogDbContext _dbContext, IWebHostEnvironment _webHostEnvironment)
        {
            this._notification = _notification;
            this._dbContext = _dbContext;
            this._webHostEnvironment = _webHostEnvironment;
        }

        [HttpGet]
        [Area("Admin")]
        public async Task<IActionResult> About()
        {
            var page = await _dbContext.pages!.FirstOrDefaultAsync(x => x.Slug=="about");
            var vm = new PageVM()
            {
                Id=page!.Id,
                Title = page.Title,
                ShortDescription = page.ShortDescription,
                Description = page.Description,
                ThumbnailUrl = page.ThumbnailUrl,
            };
            
            return View(vm);
        }

        [HttpPost]
        [Area("Admin")]
        public async Task<IActionResult> About(PageVM vm)
        {
            if (!ModelState.IsValid) { return View(vm); }
            var page = await _dbContext.pages!.FirstOrDefaultAsync(x => x.Slug == "about");
            if (page == null)
            {
                _notification.Error("Page not found!");
                return View();
            }

            page.Title = vm.Title;
            page.ShortDescription = vm.ShortDescription;
            page.Description = vm.Description;
            if (vm.Thumbnail != null)
            {
                page.ThumbnailUrl = UploadImage(vm.Thumbnail);
            }

            await _dbContext.SaveChangesAsync();
            _notification.Success("About Page updated Successfully");
            return RedirectToAction("About", "Page", new { area = "Admin" });
        }

        [HttpGet]
        [Area("Admin")]
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

        [HttpPost]
        [Area("Admin")]
        public async Task<IActionResult> Contact(PageVM vm)
        {
            if (!ModelState.IsValid) { return View(vm); }
            var page = await _dbContext.pages!.FirstOrDefaultAsync(x => x.Slug == "contact");
            if (page == null)
            {
                _notification.Error("Page not found!");
                return View();
            }

            page.Title = vm.Title;
            page.ShortDescription = vm.ShortDescription;
            page.Description = vm.Description;
            if (vm.Thumbnail != null)
            {
                page.ThumbnailUrl = UploadImage(vm.Thumbnail);
            }

            await _dbContext.SaveChangesAsync();
            _notification.Success("Contact Page updated Successfully");
            return RedirectToAction("Contact", "Page", new { area = "Admin" });
        }
        [HttpGet]
        [Area("Admin")]
        public async Task<IActionResult> Privacy()
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

        [HttpPost]
        [Area("Admin")]
        public async Task<IActionResult> Privacy(PageVM vm)
        {
            if (!ModelState.IsValid) { return View(vm); }
            var page = await _dbContext.pages!.FirstOrDefaultAsync(x => x.Slug == "privacy");
            if (page == null)
            {
                _notification.Error("Page not found!");
                return View();
            }

            page.Title = vm.Title;
            page.ShortDescription = vm.ShortDescription;
            page.Description = vm.Description;
            if (vm.Thumbnail != null)
            {
                page.ThumbnailUrl = UploadImage(vm.Thumbnail);
            }

            await _dbContext.SaveChangesAsync();
            _notification.Success("Privacy Page updated Successfully");
            return RedirectToAction("Privacy", "Page", new { area = "Admin" });
        }

        private string UploadImage(IFormFile file)
        {
            string uniqueFileName = "";
            var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "thumbnail");
            uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var filePath = Path.Combine(folderPath, uniqueFileName);
            using (FileStream fileStream = System.IO.File.Create(filePath))
            {
                file.CopyTo(fileStream);
            }
            return uniqueFileName;
        }
    }
}
