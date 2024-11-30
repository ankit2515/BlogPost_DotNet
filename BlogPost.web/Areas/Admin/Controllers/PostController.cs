using AspNetCoreHero.ToastNotification.Abstractions;
using BlogPost.web.Data;
using BlogPost.web.Models;
using BlogPost.web.Utilities;
using BlogPost.web.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;
using System.Drawing.Text;

namespace BlogPost.web.Areas.Admin.Controllers
{
    public class PostController : Controller
    {
        private readonly INotyfService _notification;
        private readonly BlogDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;
        public PostController(INotyfService _notification, BlogDbContext _dbContext, IWebHostEnvironment _webHostEnvironment, UserManager<ApplicationUser> _userManager)
        {
            this._notification = _notification;
            this._dbContext = _dbContext;
            this._webHostEnvironment = _webHostEnvironment;
            this._userManager = _userManager;
        }


        [Authorize]
        [Area("Admin")]
        public async Task<IActionResult> Index()
        {
            var listOfPosts = new List<Post>();
            var loggedInUser = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity!.Name);
            var loggedInUserRole = await _userManager.GetRolesAsync(loggedInUser!);
            if (loggedInUserRole[0] == WebsiteRoles.WebsiteAdmin)
            {
                listOfPosts = await _dbContext.posts!.Include(x => x.ApplicationUser).ToListAsync();
            }
            else
            {
                listOfPosts = await _dbContext.posts!.Include(x => x.ApplicationUser).Where(x => x.ApplicationUser!.Id == loggedInUser!.Id).ToListAsync();
            }
            var listOfPostVm = listOfPosts.Select(x => new PostVM
            {
                Id=x.Id,
                Title = x.Title,
                CreatedDate = x.CreatedDate,
                ThumbnailUrl = x.ThumbnailUrl,
                AuthorName = x.ApplicationUser!.FirstName + " " + x.ApplicationUser!.LastName,
            }).ToList();

            return View(listOfPostVm);
        }

        [HttpGet]
        [Area("admin")]
        public IActionResult createPost()
        {
            return View(new CreatePostVM());
        }

        [HttpPost]
        [Area("Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var post = await _dbContext.posts!.FirstOrDefaultAsync(x => x.Id == id);
            if (post == null)
            {
                _notification.Warning("Post cannot be null here");
                return RedirectToAction("Index");
            }

            var loggedInUser = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity!.Name);
            var loggedInUserRole = await _userManager.GetRolesAsync(loggedInUser!);

            if (loggedInUserRole[0] == WebsiteRoles.WebsiteAdmin || loggedInUser?.Id == post.ApplicationUserId)
            {
                _dbContext.posts!.Remove(post);
                await _dbContext.SaveChangesAsync();
                _notification.Information("The post has been deleted");
                return RedirectToAction("Index", "Post", new { area = "Admin" });
            }

            _notification.Error("Unable to delete the post");
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Area("admin")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var post = await _dbContext.posts!.FirstOrDefaultAsync(x => x.Id == id);

            if (post == null)
            {
                _notification.Error("post not found");
                return View();
            }

            var loggedInUser = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity!.Name);
            var loggedInUserRole = await _userManager.GetRolesAsync(loggedInUser!);

            //if user ttry to view the other's post then it can not be able to see that 
            if (loggedInUserRole[0] != WebsiteRoles.WebsiteAdmin && loggedInUser?.Id != post.ApplicationUserId)
            {
                _notification.Error("Your are not authorized");
                return RedirectToAction("Index", "Post", new { area = "Admin" });
            }

            var vm = new CreatePostVM()
            {
                Id = post.Id,
                Title= post.Title,
                Description= post.Description,
                ShortDescription= post.ShortDescription,    
                ThumbnailUrl= post.ThumbnailUrl,
            };

            return View(vm);

        }
        [HttpPost]
        [Area("admin")]
        public async Task<IActionResult> Edit(CreatePostVM vm)
        {
            if (!ModelState.IsValid) { return View(vm); }
            var post = await _dbContext.posts!.FirstOrDefaultAsync(x => x.Id == vm.Id);

            if (post == null)
            {
                _notification.Error("post not found");
                return View();
            }
            post.Title = vm.Title;
            post.Description = vm.Description;
            post.ShortDescription = vm.ShortDescription;

            if (vm.Thumbnail != null)
            {
                post.ThumbnailUrl = UploadImage(vm.Thumbnail);
            }

            await _dbContext.SaveChangesAsync();
            _notification.Success("Post Edited Successfully");
            return RedirectToAction("Index", "Post", new {area = "Admin"});

        }

        [HttpPost]
        [Area("admin")]
        public async Task<IActionResult> createPost(CreatePostVM vm)
        {
            if (!ModelState.IsValid) { return View(vm); }


            //get logged in user Id
            var loggedInUser = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity!.Name);

            var post = new Post();
            post.Title = vm.Title;
            post.Description = vm.Description;
            post.ShortDescription = vm.ShortDescription;
            post.ApplicationUserId = loggedInUser!.Id;

            if (post.Title != null)
            {
                string slug = vm.Title!.Trim();
                slug = slug.Replace(" ", "-");
                post.slug = slug + "-" + Guid.NewGuid();
            }

            if (vm.Thumbnail != null)
            {
                post.ThumbnailUrl = UploadImage(vm.Thumbnail);
            }

            await _dbContext.posts!.AddAsync(post);
            await _dbContext.SaveChangesAsync();
            _notification.Success("Post Created Successfully");
            return RedirectToAction("Index");
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
