using AspNetCoreHero.ToastNotification.Abstractions;
using BlogPost.web.Data;
using BlogPost.web.Models;
using BlogPost.web.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace BlogPost.web.Areas.Admin.Controllers
{
   
    public class SettingController : Controller
    {
      private readonly BlogDbContext dbContext;
      private readonly INotyfService notyfService;
      private readonly IWebHostEnvironment webHostEnvironment;
        public SettingController(BlogDbContext dbContext, INotyfService notyfService, IWebHostEnvironment webHostEnvironment)
        {
            this.dbContext = dbContext;
            this.notyfService = notyfService;
            this.webHostEnvironment = webHostEnvironment;   
            
        }

        [Area("Admin")]
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var setting = await dbContext.settings!.ToListAsync();
            if (setting.Count > 0) {
                var vm = new SettingVm()
                {
                    Id = setting[0].Id,
                    SiteName = setting[0].SiteName,
                    Title = setting[0].Title,
                    ShortDescription = setting[0].ShortDescription,
                    ThumbnailUrl = setting[0].ThumbnailUrl,
                    FacebookUrl = setting[0].FacebookUrl,
                    TwitterUrl = setting[0].TwitterUrl,
                    GithubUrl = setting[0].GithubUrl,

                };
                return View(vm);
            }

            var Setting = new Setting()
            {
                SiteName = "Demo Site"
            };
            
            await dbContext.settings!.AddAsync(Setting);
            await dbContext.SaveChangesAsync();

            var createdSetting = await dbContext.settings!.ToListAsync();

            var createdVm = new SettingVm() {
                Id = createdSetting[0].Id,
                SiteName = createdSetting[0].SiteName,
                Title = createdSetting[0].Title,
                ShortDescription = createdSetting[0].ShortDescription,
                ThumbnailUrl = createdSetting[0].ThumbnailUrl,
                FacebookUrl = createdSetting[0].FacebookUrl,
                TwitterUrl = createdSetting[0].TwitterUrl,
                GithubUrl = createdSetting[0].GithubUrl,

            };

            return View(createdVm);
        }

        [Area("Admin")]
        [HttpPost]
        public async Task<IActionResult> Index(SettingVm settingVm)
        {
            if (!ModelState.IsValid) { return View(settingVm); }

            var vm= await dbContext.settings!.FirstOrDefaultAsync(x => x.Id == settingVm.Id);
            if (vm == null)
            {
                notyfService.Error("Something Went Wrong");
                return View(settingVm);
            }

            vm.Id = settingVm.Id;
            vm.SiteName = settingVm.SiteName;
            vm.Title = settingVm.Title;
            vm.ShortDescription = settingVm.ShortDescription;
            if (settingVm.Thumbnail != null)
            {
                vm.ThumbnailUrl = UploadImage(settingVm.Thumbnail);
            }
            vm.FacebookUrl = settingVm.FacebookUrl;
            vm.GithubUrl = settingVm.GithubUrl;
            vm.TwitterUrl = settingVm.TwitterUrl;

            await dbContext.SaveChangesAsync();
            notyfService.Success("Setting Updated Successfully");
            return RedirectToAction("Index", "Setting", new {area = "Admin"});

        }

        private string UploadImage(IFormFile file)
        {
            string uniqueFileName = "";
            var folderPath = Path.Combine(webHostEnvironment.WebRootPath, "thumbnail");
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
